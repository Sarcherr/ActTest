using Boss;
using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;

namespace BossAct
{
    /*  Boss总生命值 float BossHP;
        Boss破防值上限 float BossTough
        Boss单次攻击韧性阈值 float minBossAtkTough;
        Boss转换阶段血量阈值 float minBossHPchangeStage;
        Boss短硬直时间 float shortBossPauseTime;
        Boss长硬直时间 float longBossPauseTime;
        啄击伤害 float BossATKHarm1;
        鸟车伤害 float BossATKHarm2;
        拍翅膀伤害 float BossATKHarm3;        */
    public class BossUpdate : BossClasses
    {
        [HideInInspector] public float preBhp;              //低于60%时切阶段
        [HideInInspector] public float preTough;
        [Header("Boss追击速度")] public float chaseSpeed;
        [Header("追击半径")] public float chaseRadius;
        [Header("攻击半径")] public float attackRadius;
        [HideInInspector] public float P_B_distance;
        [Header("鸟车冲刺速度")] public float rushSpeed;
        [Header("鸟车冲刺时间")] public float rushTime;
        [Header("Boss扔附生兽的延迟时间")] public float restTime1;
        [Header("Boss发射羽毛的延迟时间")] public float restTime2;
        [Header("Boss释放羽毛的距离阈值")] public float shootDis;
        [Header("Boss攻击冷却")] public float attackCD = 2;
        [HideInInspector] public float attackTimer;
        [HideInInspector] public bool Dir;                          //控制Boss的朝向,左为true
        [HideInInspector] public float vecDir = 1;                    //控制朝向的向量
        [HideInInspector] public bool isATK;                        //是否攻击
        [HideInInspector] public bool State;                        //判断是否进入二阶段
        [HideInInspector] public bool isIdle;                       //是否待机
        [HideInInspector] public bool isChase;                      //是否追击
        [HideInInspector] public int AttackIndex;                   //普攻序号
        [HideInInspector] public bool isHurt;                       //是否受到攻击
        [HideInInspector] public bool isHarded;                     //是否被重击
        [HideInInspector] public bool isSkillHurt;                  //是否被次元斩
        [HideInInspector] public bool isShortPaused;                //是否短硬直
        [HideInInspector] public bool isLongPaused;                 //是否长硬直
        [HideInInspector] public int isSkill;                       //是否放技能(1yes,0no)
        [HideInInspector] public float pretimer;                      //技能计时器(不能不放)
        [HideInInspector] public float pretimerTime;                  //技能计时器(不能一直放)
        [HideInInspector] public float SkillCD = 4;                 //技能CD(不能不放)
        [HideInInspector] public float SkillCD2 = 8;               //技能(不能一直放)

        [HideInInspector] public Rigidbody2D rigidbody2;    //Boss刚体
        [HideInInspector] public Animator animator;         //Boss动画机
        private BossMachine bossMachine;               //Boss状态机
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


            //我改的


            playerObject = GameObject.Find("Player");
            player = playerObject.GetComponent<Player>();


            //我改的


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
        public void EndNow()    //终止当前状态
        {
            bossMachine.presentMode.OnDestroy();
        }
        public void AddTough(int deltaTough)  //攒破防值
        {
            preTough -= deltaTough;
            if (preTough >= BossTough)  //破防值攒满后长硬直
            {
                LongPause();
                preTough = 0;
            }
        }
        public void BeHurt(int deltaHP, bool isHard)//被普攻和重击
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
            if (isHard)     //只有在受到重击时才会触发受击动画
            {
                animator.SetInteger("isPause", 1);
                EndNow();
                bossMachine.SetState(BossMode.shortPause);
                bossMachine.presentMode.OnStart();
            }
            isHurt = false; animator.SetBool(nameof(isHurt), isHurt);
        }
        public void BeSkillHurt(int deltaHP)//被次元斩
        {
            //deltaHP和deltaTough由玩家技能函数决定
            isSkillHurt = true; animator.SetBool(nameof(isSkillHurt), isSkillHurt);
            animator.SetInteger("isPause", 1);
            preBhp -= deltaHP;
            EndNow();
            bossMachine.SetState(BossMode.longPause);
            bossMachine.presentMode.OnStart();
        }
        public void ShortPause()    //短硬直,不倒地
        {
            isShortPaused = true;
            animator.SetTrigger("isShortPause");
            bossMachine.SetState(BossMode.shortPause);
            bossMachine.presentMode.OnStart();
        }
        public void LongPause()     //长硬直,硬直加倒地
        {
            isLongPaused = true;
            animator.SetTrigger("isLongPause");
            bossMachine.SetState(BossMode.longPause);
            bossMachine.presentMode.OnStart();
        }
        public void changeState()   //变红温的函数
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
            //然后爆出食材?
        }
        public void ChaseR()    //追
        {
            faceDir();
            animator.SetBool(nameof(isChase), isChase);
            rigidbody2.velocity = new Vector3(chaseSpeed * vecDir, 0, 0);
        }
        public void Idle()      //待机  
        {
            animator.SetBool(nameof(isIdle), isIdle);
        }
        public void faceDir()   //控制Boss在追击和攻击时一直朝向Player
        {
            float bossX = gameObject.transform.position.x;
            float playerX = playerObject.transform.position.x;
            //默认Boss初始状态下面朝左
            if (bossX - playerX < 0)        //Boss在Player左侧,Boss向右
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
                vecDir = 1;
                Dir = false;
            }
            else if (bossX - playerX > 0)   //Boss在Player右侧,Boss向左
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
                vecDir = -1;
                Dir = true;
            }
        }
        public void CheckState()
        {
            Vector3 Bossdir = transform.position;   //获取Boss位置
            Vector3 Playerdir = playerObject.transform.position;    //获取Player位置
            P_B_distance = Mathf.Abs(Vector3.Distance(Bossdir, Playerdir));
            if (P_B_distance <= chaseRadius && P_B_distance > attackRadius) //追
            {
                isIdle = false; animator.SetBool(nameof(isIdle), isIdle);
                isChase = true; animator.SetBool(nameof(isChase), isChase);
                EndNow();
                bossMachine.SetState(BossMode.chase);
                bossMachine.presentMode.OnStart();
            }
            else if (P_B_distance <= attackRadius && attackTimer <= 0)  //攻击
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
            int i = Random.Range(0, 2); //i=0时发射羽毛,i=1时生附生兽
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
        public IEnumerator Attack2()  //鸟车,要冲
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
        public void AttackIndexReturn() //索引归零
        {
            AttackIndex = 0;
            bossMachine.presentMode.OnDestroy();
            bossMachine.SetState(BossMode.chase);
            bossMachine.presentMode.OnStart();
        }
        public void StopRush()          //摔倒后停止运动
        {
            rigidbody2.velocity = Vector3.zero;
        }
        public void ATK1Sensor1()       //喙探测器显示
        {
            atk1_Sensor.SetActive(true);
        }
        public void ATK1Sensor0()       //喙探测器消失
        {
            atk1_Sensor.SetActive(false);
        }
        public void ATK3Sensor1()       //鸡翅探测器显示
        {
            atk3_Sensor.SetActive(true);
        }
        public void ATK3Sensor0()       //鸡翅探测器消失
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