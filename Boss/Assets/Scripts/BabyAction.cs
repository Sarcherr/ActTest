using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BabyAction : MonoBehaviour
{
    [Header("������Ѫ��")] public float babyHP;
    [Header("�������ٶ�")] public float babySpeed;
    [Header("�����޹�����")] public float babyATK;
    [Header("�������˶��ٶ�")] public float speed;
    [Header("ATK1��ɵ��˺�")] public float atk1Harm;
    [Header("ATK2��ɵ��˺�")] public float atk2Harm;
    [Header("׷���뾶,�뾶����һ��׷��,����һ�ɹ���")] public float chaseRadius;
    [HideInInspector] public bool Dir = true;    //���Ƹ����޵ĳ���,��Ϊtrue
    [HideInInspector] public Vector3 vecDir = Vector3.left;  //���Ƴ��������,Ĭ��Ϊ��
    private bool isATK;
    private bool isMove;
    private bool isHurt;
    private bool isDead;
    private float deltahp;  //��ҵĹ�����,������������ɵ��˺�

    GameObject playerObject;
    Rigidbody2D rigid;
    Animator anim;
    ATKtypes type;
    public enum ATKtypes   //�������ж�����
    {
        move = 0,   //�ƶ�(�������)
        atk1 = 1,   //��һ�ֹ�����ʽ
        atk2 = 2,   //�ڶ���
        hurt = 3,   //�ܻ�
        dead = -1    //����
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
                    SendMessage("�˴�������ܻ�����", atk1Harm);
                    anim.SetBool("BabyAtk1", isATK);
                    isATK = false;
                    isMove = true;
                    type = ATKtypes.move;
                    break;
                }
            case ATKtypes.atk2:
                {
                    isATK = true;
                    SendMessage("�˴�������ܻ�����", atk2Harm);
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
        //��ײ����ʱ������y���ٶ�����,����Ϊ��
        GameObject groundObj = collision2D.gameObject;
        float Yspeed = rigid.velocity.y;    //YspeedΪ��
        if (groundObj.name == "Ground")     //����������������
        {
            rigid.velocity = new Vector3(rigid.velocity.x, 0, transform.position.z);    //y���ٶȹ���
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
    public void faceDir()   //���Ƹ�������׷���͹���ʱһֱ����Player
    {
        float babyX = gameObject.transform.position.x;
        float playerX = playerObject.transform.position.x;
        //Ĭ�ϸ����޳�ʼ״̬���泯��
        if (babyX - playerX < 0)        //��������Player���,Boss����
        {
            transform.localScale = new Vector3(1, 1, 1);
            vecDir = Vector3.right;
            Dir = false;
        }
        else if (babyX - playerX > 0)   //��������Player�Ҳ�,Boss����
        {
            transform.localScale = new Vector3(-1, 1, 1);
            vecDir = Vector3.left;
            Dir = true;
        }
    }
}