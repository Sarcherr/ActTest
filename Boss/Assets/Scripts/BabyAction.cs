using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BabyAction : MonoBehaviour
{
    [Header("������Ѫ��")] public float babyHP;
    [Header("�������ٶ�")] public float babySpeed;
    [Header("�����ޱ�ը�˺�")] public float boomATK;
    [Header("�������˶��ٶ�")] public float speed;
    [Header("�Ա��뾶,�뾶����һ��׷��,����˲�䱬ը")] public float boomRadius;
    [HideInInspector] public bool Dir = true;    //���Ƹ����޵ĳ���,��Ϊtrue
    [HideInInspector] public Vector3 vecDir = Vector3.left;  //���Ƴ��������,Ĭ��Ϊ��
    [Header("��Ҹ���������ɵ��˺�")] private float deltahp;
    private bool isMove;
    private bool isBoom;

    BossAct.ThrowChildren throwBaby;
    GameObject playerObject;
    Rigidbody2D rigid;
    Animator anim;
    ATKtypes type;
    public enum ATKtypes   //�������ж�����
    {
        move = 0,   //�ƶ�(�������)
        boom = 1,   //������Һ��Ա�
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
        //��ײ����ʱ������y���ٶ�����,����Ϊ��
        GameObject groundObj = collision2D.gameObject;
        float Yspeed = rigid.velocity.y;    //YspeedΪ��
        if (groundObj.name == "Ground")     //����������������
        {
            rigid.velocity = new Vector3(rigid.velocity.x, 0, transform.position.z);    //y���ٶȹ���
        }
    }
    void ReachPlayer()  //�������,�ж��ƶ����Ա�
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
            rigid.AddForce(vecDir * -1 * speed / 5, 0);    //��ͣ��
        }
        else
        {
            isBoom = true; anim.SetBool("selfBoom", isBoom);
            gameObject.SetActive(false);
            throwBaby.num--;
        }
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