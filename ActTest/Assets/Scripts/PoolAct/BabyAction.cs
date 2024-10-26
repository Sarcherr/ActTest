using BossAct;
using System;
using System.Collections;
using System.Collections.Generic;
using Unit;
using Unity.VisualScripting;
using UnityEngine;

public class BabyAction : MonoBehaviour
{
    [Header("������Ѫ��")] public float babyHP;
    [Header("�����ޱ�ը�˺�")] public int boomATK;
    [Header("�������˶��ٶ�")] public float speed;
    [Header("�����Ա��뾶")] public float boomRadius;
    [HideInInspector] public bool Dir = true;    //���Ƹ����޵ĳ���,��Ϊtrue
    [HideInInspector] public Vector3 vecDir = Vector3.left;  //���Ƴ��������,Ĭ��Ϊ��
    private bool isChase;
    private bool isBoom;
    private bool OnBorn;
    private bool isHurt;
    [Header("��Ӳֱʱ��")] public float PauseTime;
    [Header("���˷���")] public float Bone;

    ThrowChildren throwBaby;
    GameObject playerObject;
    Rigidbody2D rigid;
    [HideInInspector] public Animator anim;
    Player player;
    [HideInInspector] public ATKtypes type;
    public enum ATKtypes   //�������ж�����
    {
        move = 0,   //�ƶ�(�������)
        boom = 1,   //������Һ��Ա�
        born = 2,   //����!
        hurt = 3,   //������,Ӳֱ
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
    void ReachPlayer()  //�������,�ж��ƶ����Ա�
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
    public void faceDir()   //���Ƹ�������׷���͹���ʱһֱ����Player
    {
        float babyX = gameObject.transform.position.x;
        float playerX = playerObject.transform.position.x;
        //Ĭ�ϸ����޳�ʼ״̬���泯��
        if (babyX - playerX < 0)        //��������Player���,Boss����
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
            vecDir = Vector3.right;
            Dir = false;
        }
        else if (babyX - playerX > 0)   //��������Player�Ҳ�,Boss����
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