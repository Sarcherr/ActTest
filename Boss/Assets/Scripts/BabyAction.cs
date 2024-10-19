using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BabyAction : MonoBehaviour
{
    [Header("附生兽血量")] public float babyHP;
    [Header("附生兽速度")] public float babySpeed;
    [Header("附生兽爆炸伤害")] public float boomATK;
    [Header("附生兽运动速度")] public float speed;
    [Header("自爆半径,半径以外一律追击,以内瞬间爆炸")] public float boomRadius;
    [HideInInspector] public bool Dir = true;    //控制附生兽的朝向,左为true
    [HideInInspector] public Vector3 vecDir = Vector3.left;  //控制朝向的向量,默认为左
    [Header("玩家给附生兽造成的伤害")] private float deltahp;
    private bool isMove;
    private bool isBoom;

    BossAct.ThrowChildren throwBaby;
    GameObject playerObject;
    Rigidbody2D rigid;
    Animator anim;
    ATKtypes type;
    public enum ATKtypes   //附生兽行动类型
    {
        move = 0,   //移动(靠近玩家)
        boom = 1,   //靠近玩家后自爆
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
        ReachPlayer();
        switch (type)
        {
            case ATKtypes.move:
                {
                    Move(); break;
                }     
            case ATKtypes.boom:
                {
                    Boom(); break;
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
    void ReachPlayer()  //计算距离,判断移动或自爆
    {
        Vector3 babyPos = transform.position;
        Vector3 playerPos = playerObject.transform.position;
        float distance = (babyPos - playerPos).magnitude;
        if (distance <= boomRadius)
        {
            type = ATKtypes.boom;
        }
        else
        {
            type = ATKtypes.move;
        }
    }
    public void Move()
    {
        isMove = true; anim.SetBool("isMove", isMove);
        isBoom = false; anim.SetBool("isATK", isBoom);
        faceDir();
        transform.position += speed * Time.deltaTime * vecDir;
    }
    public void Boom()
    {
        isMove = false; anim.SetBool("isMove", isMove);
        if (rigid.velocity.x != 0)
        {
            rigid.AddForce(vecDir * -1 * speed / 5, 0);    //逐渐停下
        }
        else
        {
            isBoom = true; anim.SetBool("selfBoom", isBoom);
            gameObject.SetActive(false);
            throwBaby.num--;
        }
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