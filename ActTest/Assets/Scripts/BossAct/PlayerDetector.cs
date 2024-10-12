using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerDetector : MonoBehaviour
{
    public float searchSpeed;   //巡逻速度
    public float chaseRadius;   //追击半径
    public float chaseSpeed;    //追击速度
    public float attackRadius;  //攻击半径
    private float P_B_distance; //Boss与Player的距离

    [HideInInspector]public float leftBorder = 100;    //Boss移动左边界
    [HideInInspector]public float rightBorder = 200;   //Boss移动右边界(数值后期改)
    public float searchTimer = 0;  //巡逻时到达边界后延时转向

    private bool isChase = false;
    private bool isAttack = false;
    private bool isSearch = true;
    [HideInInspector]public bool Dir = true;    //控制Boss的朝向,左为true
    [HideInInspector]public Vector3 vecDir = Vector3.left;  //控制朝向的向量,默认为左

    GameObject playerObject = GameObject.Find("Player");    //找寻player,后面换玩家object的名字
    Animator animator;
    void Start()
    {
        Transform transform = GetComponent<Transform>();       
        Rigidbody2D rig2D = transform.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Vector3 Bossdir = transform.position;   //获取Boss位置
        Vector3 Playerdir = playerObject.transform.position;    //获取Player位置
        P_B_distance = Vector3.Distance(Bossdir, Playerdir);
        if (P_B_distance <= chaseRadius && P_B_distance > attackRadius)
        {
            isSearch = false;
            isChase = true;
            isAttack = false;
            ChaseR();
        }else if (P_B_distance <= chaseRadius)
        {
            isSearch = false;
            isChase = false;
            isAttack = true;
            AttackR();
        }else if (P_B_distance > chaseRadius)
        {
            isSearch = true;
            isChase = false;
            isAttack = false;
            Search();
        }
    }
    public void ChaseR()
    {
        faceDir();
        animator = GetComponent<Animator>();
        animator.SetBool("isChase", isChase);
        transform.position += chaseSpeed * Time.deltaTime * vecDir;
    }
    public void AttackR()
    {
        faceDir();
        animator = GetComponent<Animator>();
        animator.SetBool("isAttack", isAttack);
    }
    public void Search()
    {
        Vector3 direction = new Vector3();
        animator = GetComponent<Animator>();
        animator.SetBool("isSearch", isSearch);
        if (Dir)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.position += searchSpeed * Time.deltaTime * Vector3.left;
        }
        else if (!Dir)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.position += searchSpeed * Time.deltaTime * Vector3.right;
        }
        Vector3 BossPos = transform.position + direction * searchSpeed * Time.deltaTime;
        BossPos.x = Mathf.Clamp(BossPos.x, leftBorder, rightBorder);
        if (transform.position.x <= leftBorder || transform.position.x >= rightBorder)
        {
            Dir = !Dir;
            searchTimer = 1;    //依实际情况改数字
        }
        if (searchTimer > 0)
        {   //延时操作的关键在这里!
            searchTimer -= Time.deltaTime;
        }
        else if (searchTimer < 0)
        {
            searchTimer = 0;
        }
    }
    public void faceDir()   //控制Boss在追击和攻击时一直朝向Player
    {
        float bossX = gameObject.transform.position.x;
        float playerX = playerObject.transform.position.x;
        //默认Boss初始状态下面朝左
        if (bossX - playerX < 0)        //Boss在Player左侧,Boss向右
        {
            transform.localScale = new Vector3(1, 1, 1);
            vecDir = Vector3.right;
        }
        else if (bossX - playerX > 0)   //Boss在Player右侧,Boss向左
        {
            transform.localScale = new Vector3(-1, 1, 1);
            vecDir = Vector3.left;
        }
    }
}