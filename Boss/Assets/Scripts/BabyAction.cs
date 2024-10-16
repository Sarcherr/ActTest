using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BabyAction : MonoBehaviour
{
    [Header("附生兽血量")] public float babyHP;
    [Header("附生兽速度")] public float babySpeed;
    [Header("附生兽攻击力")] public float babyATK;
    [Header("附生兽运动速度")] public float speed;
    [Header("ATK1造成的伤害")] public float atk1Harm;
    [Header("ATK2造成的伤害")] public float atk2Harm;
    [Header("追击半径,半径以外一律追击,以内一律攻击")] public float chaseRadius;
    [HideInInspector] public bool Dir = true;    //控制附生兽的朝向,左为true
    [HideInInspector] public Vector3 vecDir = Vector3.left;  //控制朝向的向量,默认为左
    private bool isATK;
    private bool isMove;
    private bool isHurt;
    private bool isDead;
    private float deltahp;  //玩家的攻击力,即给附生兽造成的伤害

    GameObject playerObject;
    Rigidbody2D rigid;
    Animator anim;
    ATKtypes type;
    public enum ATKtypes   //附生兽行动类型
    {
        move = 0,   //移动(靠近玩家)
        atk1 = 1,   //第一种攻击方式
        atk2 = 2,   //第二种
        hurt = 3,   //受击
        dead = -1    //死亡
    }
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Transform transform = GetComponent<Transform>();
        playerObject = GameObject.Find("Player");
        anim = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        if (isMove)
        {
            type = ATKtypes.move;
        }
        switch (type)
        {
            case ATKtypes.move:
                {
                    Move();
                    break;
                }
            case ATKtypes.atk1:
                {
                    isATK = true;
                    SendMessage("此处填玩家受击函数", atk1Harm);
                    anim.SetBool("BabyAtk1", isATK);
                    isATK = false;
                    isMove = true;
                    type = ATKtypes.move;
                    break;
                }
            case ATKtypes.atk2:
                {
                    isATK = true;
                    SendMessage("此处填玩家受击函数", atk2Harm);
                    anim.SetBool("BabyAtk2", isATK);
                    isATK = false;
                    isMove = true;
                    type = ATKtypes.move;
                    break;
                }
            case ATKtypes.hurt:
                {
                    isHurt = true;
                    babyHP -= deltahp;
                    anim.SetBool("BabyHurt", isHurt);
                    isHurt = false;
                    isMove = true;
                    if (babyHP <= 0)
                    {
                        type = ATKtypes.dead;
                        isDead = true;
                    }
                    break;
                }
            case ATKtypes.dead:
                {
                    anim.SetBool("BabyDead", isDead);
                    Destroy(gameObject);
                    break;
                }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        //碰撞触发时附生兽y轴速度向下,速率为负
        GameObject groundObj = collision2D.gameObject;
        float Yspeed = rigid.velocity.y;    //Yspeed为负
        if (groundObj.name == "Ground")     //当附生兽碰到地面
        {
            rigid.velocity = new Vector3(rigid.velocity.x, 0, transform.position.z);    //y轴速度归零
        }
    }
    void ReachPlayer()
    {
        Vector3 babyPos = transform.position;
        Vector3 playerPos = playerObject.transform.position;
        float distance = (babyPos - playerPos).magnitude;
        if (distance <= chaseRadius)
        {
            int ATKindex = Random.Range(0, 2);
            if (ATKindex == 0)
            {
                type = ATKtypes.atk1;
            }else if (ATKindex == 1)
            {
                type = ATKtypes.atk2;
            }
        }else
        {
            type = ATKtypes.move;
        }
    }
    public void Move()
    {
        faceDir();
        anim.SetBool("BabyMove", true);
        transform.position += speed * Time.deltaTime * vecDir;
    }
    public void faceDir()   //控制附生兽在追击和攻击时一直朝向Player
    {
        float babyX = gameObject.transform.position.x;
        float playerX = playerObject.transform.position.x;
        //默认附生兽初始状态下面朝左
        if (babyX - playerX < 0)        //附生兽在Player左侧,Boss向右
        {
            transform.localScale = new Vector3(1, 1, 1);
            vecDir = Vector3.right;
            Dir = false;
        }
        else if (babyX - playerX > 0)   //附生兽在Player右侧,Boss向左
        {
            transform.localScale = new Vector3(-1, 1, 1);
            vecDir = Vector3.left;
            Dir = true;
        }
    }
}