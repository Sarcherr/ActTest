using BossAct;
using System;
using System.Collections;
using System.Collections.Generic;
using Unit;
using Unity.VisualScripting;
using UnityEngine;

public class BabyAction : MonoBehaviour
{
    [Header("附生兽血量")] public float babyHP;
    [Header("附生兽爆炸伤害")] public int boomATK;
    [Header("附生兽运动速度")] public float speed;
    [Header("触发自爆半径")] public float boomRadius;
    [HideInInspector] public bool Dir = true;    //控制附生兽的朝向,左为true
    [HideInInspector] public Vector3 vecDir = Vector3.left;  //控制朝向的向量,默认为左
    private bool isChase;
    private bool isBoom;
    private bool OnBorn;
    private bool isHurt;
    [Header("长硬直时间")] public float PauseTime;
    [Header("击退幅度")] public float Bone;

    ThrowChildren throwBaby;
    GameObject playerObject;
    Rigidbody2D rigid;
    [HideInInspector] public Animator anim;
    Player player;
    [HideInInspector] public ATKtypes type;
    public enum ATKtypes   //附生兽行动类型
    {
        move = 0,   //移动(靠近玩家)
        boom = 1,   //靠近玩家后自爆
        born = 2,   //出生!
        hurt = 3,   //被攻击,硬直
    }
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Transform transform = GetComponent<Transform>();
        playerObject = GameObject.Find("Player");
        OnBorn = false;
        isHurt = false;
        type = ATKtypes.born;
    }
    void FixedUpdate()
    {
        //ReachPlayer();
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
            case ATKtypes.born:
                {
                    Born(); break;
                }
            //case ATKtypes.hurt:
            //    {
            //        Hurt(); break;
            //    }
        }
    }
    void ReachPlayer()  //计算距离,判断移动或自爆
    {
        Vector3 babyPos = transform.position;
        Vector3 playerPos = playerObject.transform.position;
        float distance = Mathf.Abs((babyPos - playerPos).magnitude);
        if (distance >= boomRadius)
        {
            type = ATKtypes.move;
        }
    }
    public void Move()
    {
        Vector3 babyPos = transform.position;
        Vector3 playerPos = playerObject.transform.position;
        vecDir = (playerPos - babyPos).normalized;
        anim.SetBool(nameof(isChase), isChase);
        //faceDir();
        Debug.Log(vecDir);
        transform.position += speed * Time.deltaTime * vecDir;
    }
    public void Boom()
    {
        isChase = false; anim.SetBool(nameof(isChase), isChase);
        isBoom = true; anim.SetTrigger(nameof(isBoom));
        if (!isBoom)
        {
            throwBaby.num--;
            Destroy(gameObject);
        }
    }
    public void Born()
    {
        OnBorn = true; anim.SetTrigger(nameof(OnBorn));
        if (!OnBorn)
        {
            type = ATKtypes.move;
        }
    }
    public void BeHurt(int damage, bool isHard)
    {
        if (isHard)
        {
            rigid.AddForce(new Vector2(-vecDir.x * Bone * 1.5f, 0), ForceMode2D.Impulse);
        }
        else
        {
            rigid.AddForce(new Vector2(-vecDir.x * Bone, 0), ForceMode2D.Impulse);
        }
        isHurt = true; anim.SetTrigger(nameof(isHurt));
        babyHP -= damage;
        if (!isHurt)
        {
            type = ATKtypes.move;
        }
    }
    public void BeSkillHurt(int damage)
    { 
        isHurt = true; anim.SetTrigger(nameof(isHurt));
        anim.speed = 0.05f;
        babyHP -= damage;
        if (!isHurt)
        {
            rigid.AddForce(new Vector2(-vecDir.x * Bone * 1.5f, 0), ForceMode2D.Impulse);
            type = ATKtypes.move;
        }
    }
    public void faceDir()   //控制附生兽在追击和攻击时一直朝向Player
    {
        float babyX = gameObject.transform.position.x;
        float playerX = playerObject.transform.position.x;
        //默认附生兽初始状态下面朝左
        if (babyX - playerX < 0)        //附生兽在Player左侧,Boss向右
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
            vecDir = Vector3.right;
            Dir = false;
        }
        else if (babyX - playerX > 0)   //附生兽在Player右侧,Boss向左
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            vecDir = Vector3.left;
            Dir = true;
        }
    }
    public void SetOnBorn(int ifTrue)
    {
        OnBorn = Convert.ToBoolean(ifTrue);
        if (ifTrue == 0)
        {
            type = ATKtypes.move;
            isChase = true;
        }
    }
    public void OnHurt(int ifHurt)
    {
        isHurt = Convert.ToBoolean(ifHurt);
        if (!isHurt)
        {
            anim.speed = 1;
        }
    }
    public void SelfDestroy()
    {
        Destroy(gameObject); 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Hurt(boomATK);
            type = ATKtypes.boom;
        }
    }
}