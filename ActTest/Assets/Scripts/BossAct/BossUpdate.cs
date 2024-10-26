using Boss;
using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;

namespace BossAct
{
    /*  Boss������ֵ float BossHP;
        Boss�Ʒ�ֵ���� float BossTough
        Boss���ι���������ֵ float minBossAtkTough;
        Bossת���׶�Ѫ����ֵ float minBossHPchangeStage;
        Boss��Ӳֱʱ�� float shortBossPauseTime;
        Boss��Ӳֱʱ�� float longBossPauseTime;
        �Ļ��˺� float BossATKHarm1;
        ���˺� float BossATKHarm2;
        �ĳ���˺� float BossATKHarm3;        */
    public class BossUpdate : BossClasses
    {
        [HideInInspector] public float preBhp;              //����60%ʱ�н׶�
        [HideInInspector] public float preTough;
        [Header("Boss׷���ٶ�")] public float chaseSpeed;
        [Header("׷���뾶")] public float chaseRadius;
        [Header("�����뾶")] public float attackRadius;
        [HideInInspector] public float P_B_distance;
        [Header("�񳵳���ٶ�")] public float rushSpeed;
        [Header("�񳵳��ʱ��")] public float rushTime;
        [Header("Boss�Ӹ����޵��ӳ�ʱ��")] public float restTime1;
        [Header("Boss������ë���ӳ�ʱ��")] public float restTime2;
        [Header("Boss�ͷ���ë�ľ�����ֵ")] public float shootDis;
        [Header("Boss������ȴ")] public float attackCD = 2f;
        [HideInInspector] public float attackTimer;
        [HideInInspector] public bool Dir;                          //����Boss�ĳ���,��Ϊtrue
        [HideInInspector] public float vecDir = 1;                    //���Ƴ��������
        [HideInInspector] public bool isATK;                        //�Ƿ񹥻�
        [HideInInspector] public bool State;                        //�ж��Ƿ������׶�
        [HideInInspector] public bool isIdle;                       //�Ƿ����
        [HideInInspector] public bool isChase;                      //�Ƿ�׷��
        [HideInInspector] public int AttackIndex;                   //�չ����
        [HideInInspector] public bool isHurt;                       //�Ƿ��ܵ�����
        [HideInInspector] public bool isHarded;                     //�Ƿ��ػ�
        [HideInInspector] public bool isSkillHurt;                  //�Ƿ񱻴�Ԫն
        [HideInInspector] public bool isShortPaused;                //�Ƿ��Ӳֱ
        [HideInInspector] public bool isLongPaused;                 //�Ƿ�Ӳֱ
        [HideInInspector] public int isSkill;                       //�Ƿ�ż���(1yes,0no)
        [HideInInspector] public float pretimer;                      //���ܼ�ʱ��(���ܲ���)
        [HideInInspector] public float pretimerTime;                  //���ܼ�ʱ��(����һֱ��)
        [HideInInspector] public float SkillCD = 5;                 //����CD(���ܲ���)
        [HideInInspector] public float SkillCD2 = 10;               //����(����һֱ��)

        [HideInInspector] public Rigidbody2D rigidbody2;    //Boss����
        [HideInInspector] public Animator animator;         //Boss������
        private BossMachine bossMachine;               //Boss״̬��
        [HideInInspector] public GameObject playerObject;
        [HideInInspector] public GameObject atk1_Sensor;
        [HideInInspector] public GameObject atk3_Sensor;
        [HideInInspector] public bool Onhit = false;
        private Player player;
        private BabyAction action;
        private ThrowChildren throwBaby;
        private Barrage shootFeather;
        private Collider2D collider2d;
        private void Start()
        {
            AttackIndex = 0;
            Dir = true;
            isATK = false;
            State = false;
            isIdle = true;
            isChase = false;
            isHurt = false;
            isHarded = false;
            isSkillHurt = false;
            isShortPaused = false;
            isLongPaused = false;
            preBhp = BossHP;
            preTough = BossTough;
            rigidbody2 = GetComponent<Rigidbody2D>();
            throwBaby = GetComponent<ThrowChildren>();
            bossMachine = new BossMachine(gameObject);
            shootFeather = GetComponent<Barrage>();
            atk1_Sensor = transform.Find(nameof(atk1_Sensor)).gameObject;
            atk3_Sensor = transform.Find(nameof(atk3_Sensor)).gameObject;
            animator = GetComponent<Animator>();
            player = GetComponent<Player>();
            collider2d = GetComponent<Collider2D>();
            bossMachine.SetUp();

            //Instantiate(gameObject);
        }
        private void Update()
        {
            bossMachine.OnUpdate();
            pretimer += Time.deltaTime;
            if (pretimer > SkillCD && State && pretimerTime <= 0)
            {
                bossMachine.presentMode.OnDestroy();
                bossMachine.SetState(BossMode.skillAttack);
                bossMachine.presentMode.OnStart();
                pretimer = 0;
                pretimerTime = SkillCD2;
            }
            if (pretimerTime > 0)
            {
                pretimerTime -= Time.deltaTime;
            }
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
        }
        private void FixedUpdate()
        {
            bossMachine.OnFixedUpdate();
        }
        public void EndNow()    //��ֹ��ǰ״̬
        {
            bossMachine.presentMode.OnDestroy();
        }
        public void AddTough(int deltaTough)  //���Ʒ�ֵ
        {
            preTough -= deltaTough;
            if (preTough >= BossTough)  //�Ʒ�ֵ������Ӳֱ
            {
                LongPause();
                preTough = 0;
            }
        }
        public void BeHurt(int deltaHP, bool isHard)//���չ����ػ�
        {
            bossMachine.SetState(BossMode.hurt);
            preBhp -= deltaHP;
            isHurt = true; animator.SetBool(nameof(isHurt), isHurt);
            isATK = false; animator.SetBool(nameof(isATK), isATK);
            if (preBhp < BossHP * 0.6f && !State)
            {
                changeState();
            }
            if (preBhp <= 0)
            {
                EndNow();
                bossMachine.SetState(BossMode.dead);
                bossMachine.presentMode.OnStart();
            }
            if (isHard)     //ֻ�����ܵ��ػ�ʱ�Żᴥ���ܻ�����
            {
                animator.SetInteger("isPause", 1);
                EndNow();
                bossMachine.SetState(BossMode.shortPause);
                bossMachine.presentMode.OnStart();
            }
            isHurt = false; animator.SetBool(nameof(isHurt), isHurt);
        }
        public void BeSkillHurt(int deltaHP)//����Ԫն
        {
            //deltaHP��deltaTough����Ҽ��ܺ�������
            isSkillHurt = true; animator.SetBool(nameof(isSkillHurt), isSkillHurt);
            animator.SetInteger("isPause", 1);
            preBhp -= deltaHP;
            EndNow();
            bossMachine.SetState(BossMode.longPause);
            bossMachine.presentMode.OnStart();
        }
        public void ShortPause()    //��Ӳֱ,������
        {
            isShortPaused = true;
            animator.SetTrigger("isShortPause");
            bossMachine.SetState(BossMode.shortPause);
            bossMachine.presentMode.OnStart();
        }
        public void LongPause()     //��Ӳֱ,Ӳֱ�ӵ���
        {
            isLongPaused = true;
            animator.SetTrigger("isLongPause");
            bossMachine.SetState(BossMode.longPause);
            bossMachine.presentMode.OnStart();
        }
        public void changeState()   //����µĺ���
        {
            if (preBhp <= 0.6f * BossHP)
            {
                State = true;
            }
        }
        public void Dead()
        {
            animator.SetBool("isDead", true);
            gameObject.SetActive(false);
            //Ȼ�󱬳�ʳ��?
        }
        public void ChaseR()    //׷
        {
            faceDir();
            animator.SetBool(nameof(isChase), isChase);
            rigidbody2.velocity = new Vector3(chaseSpeed * vecDir, 0, 0);
        }
        public void Idle()      //����  
        {
            animator.SetBool(nameof(isIdle), isIdle);
        }
        public void faceDir()   //����Boss��׷���͹���ʱһֱ����Player
        {
            float bossX = gameObject.transform.position.x;
            float playerX = playerObject.transform.position.x;
            //Ĭ��Boss��ʼ״̬���泯��
            if (bossX - playerX < 0)        //Boss��Player���,Boss����
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
                vecDir = 1;
                Dir = false;
            }
            else if (bossX - playerX > 0)   //Boss��Player�Ҳ�,Boss����
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
                vecDir = -1;
                Dir = true;
            }
        }
        public void CheckState()
        {
            Vector3 Bossdir = transform.position;   //��ȡBossλ��
            Vector3 Playerdir = playerObject.transform.position;    //��ȡPlayerλ��
            P_B_distance = Mathf.Abs(Vector3.Distance(Bossdir, Playerdir));
            if (P_B_distance <= chaseRadius && P_B_distance > attackRadius) //׷
            {
                isIdle = false; animator.SetBool(nameof(isIdle), isIdle);
                isChase = true; animator.SetBool(nameof(isChase), isChase);
                EndNow();
                bossMachine.SetState(BossMode.chase);
                bossMachine.presentMode.OnStart();
            }
            else if (P_B_distance <= attackRadius && attackTimer <= 0)  //����
            {
                isIdle = false; animator.SetBool(nameof(isIdle), isIdle);
                isChase = false; animator.SetBool(nameof(isChase), isChase);
                isATK = true; animator.SetBool(nameof(isATK), isATK);
                if (!State)
                {
                    EndNow();
                    bossMachine.SetState(BossMode.attack);
                    bossMachine.presentMode.OnStart();
                }
                else if (State)
                {
                    EndNow();
                    isSkill = Random.Range(0, 2);
                    if (isSkill == 1)
                    {
                        if (pretimer > SkillCD && State && pretimerTime <= 0)
                        {
                            bossMachine.SetState(BossMode.skillAttack);
                            bossMachine.presentMode.OnStart();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (isSkill == 0)
                    {
                        bossMachine.SetState(BossMode.attack);
                        bossMachine.presentMode.OnStart();
                    }
                }
            }
        }
        public void DecideSkillAtk()
        {
            int i = Random.Range(0, 2); //i=0ʱ������ë,i=1ʱ��������
            if (i == 0)
            {
                int babynum = throwBaby.num;
                if (babynum <= 1)
                {
                    throwBaby.Invoke(nameof(throwBaby.OnThrow), restTime1);
                    Debug.Log("throw");
                }
                else if (babynum > 1)
                {
                    return;
                }
            }
            else if (i == 1)
            {
                if (shootDis >= P_B_distance)
                {
                    animator.SetTrigger("shootFeather");
                    animator.Play("Boss_ShootFeather");
                    shootFeather.Invoke(nameof(shootFeather.ShootFeather), restTime2);
                    Debug.Log("shoot");
                }
                else if (shootDis < P_B_distance)
                {
                    return;
                }
            }
        }
        public void Attack1()
        {
            animator.SetInteger(nameof(AttackIndex), AttackIndex);
        }
        public IEnumerator Attack2()  //��,Ҫ��
        {
            float pretime = 0;
            AttackIndex = 2; animator.SetInteger(nameof(AttackIndex), AttackIndex);
            while (pretime <= rushTime)
            {
                rigidbody2.velocity = new Vector3(vecDir * rushSpeed, 0, 0);
                pretime += Time.deltaTime;
            }
            yield return null;
        }
        public void Attack3()
        {
            animator.SetInteger(nameof(AttackIndex), AttackIndex);
        }
        public void AttackIndexReturn() //��������
        {
            AttackIndex = 0;
            bossMachine.presentMode.OnDestroy();
            bossMachine.SetState(BossMode.chase);
            bossMachine.presentMode.OnStart();
        }
        public void StopRush()          //ˤ����ֹͣ�˶�
        {
            rigidbody2.velocity = Vector3.zero;
        }
        public void ATK1Sensor1()       //�̽������ʾ
        {
            atk1_Sensor.SetActive(true);
        }
        public void ATK1Sensor0()       //�̽������ʧ
        {
            atk1_Sensor.SetActive(false);
        }
        public void ATK3Sensor1()       //����̽������ʾ
        {
            atk3_Sensor.SetActive(true);
        }
        public void ATK3Sensor0()       //����̽������ʧ
        {
            atk3_Sensor.SetActive(false);
        }
        public void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && isATK && !Onhit)
            {
                if (AttackIndex == 1)
                {
                    Debug.Log("1");
                    collision.GetComponent<Player>().Hurt(BossATKHarm1);
                    Onhit = true;
                }
                else if (AttackIndex == 2)
                {
                    Debug.Log("2");
                    collision.GetComponent<Player>().Hurt(BossATKHarm2);
                    Onhit = true;
                }
                else if (AttackIndex == 3)
                {
                    Debug.Log("3");
                    collision.GetComponent<Player>().Hurt(BossATKHarm3);
                    Onhit = true;
                }
            }
        }
    }
}