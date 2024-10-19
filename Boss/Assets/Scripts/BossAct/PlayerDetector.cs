using Boss;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerDetector : MonoBehaviour
{
    public float searchSpeed;   //Ѳ���ٶ�
    public float chaseRadius;   //׷���뾶
    public float chaseSpeed;    //׷���ٶ�
    public float attackRadius;  //�����뾶
    private float P_B_distance; //Boss��Player�ľ���

    [HideInInspector]public float leftBorder = 100;    //Boss�ƶ���߽�
    [HideInInspector]public float rightBorder = 200;   //Boss�ƶ��ұ߽�(��ֵ���ڸ�)
    public float searchTimer = 0;  //Ѳ��ʱ����߽����ʱת��

    private bool isChase = false;
    private bool isSearch = true;
    private bool hasDetected = false;   //�ڷ�����Һ���Ϊtrue,��ʱ���ٽ���Ѳ��״̬
    public bool isState = false;       //�ж��Ƿ������׶�

    [HideInInspector]public bool Dir = true;    //����Boss�ĳ���,��Ϊtrue
    [HideInInspector]public Vector3 vecDir = Vector3.left;  //���Ƴ��������,Ĭ��Ϊ��
    GameObject playerObject;
    Animator animator;
    private BossAct.BossUpdate bossUpdate;
    private BossAct.BossAttackMotions BossATKmotions;
    private BossAct.BossAttackMotions2 BossATKMotions2;
    void Start()
    {
        Transform transform = GetComponent<Transform>();       
        Rigidbody2D rig2D = transform.GetComponent<Rigidbody2D>();
        bossUpdate = GetComponent<BossAct.BossUpdate>();
        BossATKmotions = GetComponent<BossAct.BossAttackMotions>();
        animator = GetComponent<Animator>();
        playerObject = GameObject.Find("Player");    //��Ѱplayer,���滻���object������
        animator.SetInteger("isPause", 0);
    }
    void FixedUpdate()   
    {
        Vector3 Bossdir = transform.position;   //��ȡBossλ��
        Vector3 Playerdir = playerObject.transform.position;    //��ȡPlayerλ��
        P_B_distance = Vector3.Distance(Bossdir, Playerdir);
        if (P_B_distance <= chaseRadius && P_B_distance > attackRadius) //׷
        {
            hasDetected = true; animator.SetBool("hasDetected", hasDetected);
            isSearch = false; animator.SetBool("isSearch", isSearch);
            isChase = true; animator.SetBool("isChase", isChase);
            ChaseR();
        }
        else if (P_B_distance <= chaseRadius)  //����
        {
            isSearch = false;
            isChase = false;
            if (!isState)
            {
                BossATKmotions.FixedUpdate();
            }
            else if (isState)
            {
                int i = Random.Range(0, 2);
                if (i == 0)
                {
                    BossATKmotions.FixedUpdate();
                }
                else if (i == 1)
                {
                    BossATKMotions2.FixedUpdate();
                }
            }
        }    
        else if (P_B_distance > chaseRadius)   //Ѳ��
        {
            isSearch = true;
            isChase = false;
            if (hasDetected)
            {
                ChaseR();
            }
            else if (!hasDetected)
            {
                Search();
            }
        }
    }
    public void ChaseR()
    {
        faceDir();
        bossUpdate.ToughRecovery();
        animator.SetBool("isChase", isChase);
        transform.position += chaseSpeed * Time.deltaTime * vecDir;
    }
    public void Search()
    {
        bossUpdate.ToughRecovery();
        Vector3 direction = new Vector3();
        animator.SetBool("searchAnim", isSearch);
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
            searchTimer = 1;    //��ʵ�����������
        }
        if (searchTimer > 0)
        {   //��ʱ�����Ĺؼ�������!
            searchTimer -= Time.deltaTime;
        }
        else if (searchTimer < 0)
        {
            searchTimer = 0;
        }
    }
    public void faceDir()   //����Boss��׷���͹���ʱһֱ����Player
    {
        float bossX = gameObject.transform.position.x;
        float playerX = playerObject.transform.position.x;
        //Ĭ��Boss��ʼ״̬���泯��
        if (bossX - playerX < 0)        //Boss��Player���,Boss����
        {
            transform.localScale = new Vector3(1, 1, 1);
            vecDir = Vector3.right;
            Dir = false;
        }
        else if (bossX - playerX > 0)   //Boss��Player�Ҳ�,Boss����
        {
            transform.localScale = new Vector3(-1, 1, 1);
            vecDir = Vector3.left;
            Dir = true;
        }
    }
}