using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;

namespace Unit
{
    //继承的数据成员:
    //(最大生命值)int maxHP;
    //(移动速度)  float moveSpeed;
    //(跳跃力度)  float jumpForce
    //(朝向)      int faceDir;
    //(当前生命值)int currentHP;
    //(是否触地)  bool isGrounded;
    //(触地传感器)GroundSensor groundSensor;
    /// <summary>
    /// 玩家角色类
    /// </summary>
    public class Player : Unit
    {
        /// <summary>
        /// 下坠重力
        /// </summary>
        [Header("下坠重力")] public float fallGravity;
        /// <summary>
        /// 最大下坠速度
        /// </summary>
        [Header("最大下坠速度")] public float maxFallSpeed;

        /// <summary>
        /// 攻击连段窗口期
        /// </summary>
        [Header("攻击连段窗口期")] public float attackTime;
        /// <summary>
        /// 普通(轻)攻击伤害
        /// </summary>
        [Header("普通(轻)攻击伤害")] public int attackDamage_normal;
        /// <summary>
        /// 普通(轻)攻击移动幅度
        /// </summary>
        [Header("普通(轻)攻击移动幅度")] public float attackMoveForce_normal = 1.5f;

        /// <summary>
        /// 闪避速度
        /// </summary>
        [Header("闪避速度")] public float dashSpeed;
        //闪避时间
        //[Header("闪避时间")] public float dashTime;
        //闪避窗口期
        //[Header("闪避窗口期")] public float dashWindow;
        /// <summary>
        /// 闪避冷却    
        /// </summary>
        [Header("闪避冷却")] public float dashCold;
        /// <summary>
        /// 前闪获得架势值
        /// </summary>
        [Header("前闪获得架势值")] public int dashGain_SP;
        /// <summary>
        /// 后闪获得血瓶条
        /// </summary>
        [Header("后闪获得血瓶条")] public int dashGain_Cure;

        /// <summary>
        /// 最大架势值
        /// </summary>
        [Header("最大架势值")] public int maxSP;
        /// <summary>
        /// 最大血瓶条
        /// </summary>
        [Header("最大血瓶条")] public int maxCure;

        /// <summary>
        /// 预输入窗口    
        /// </summary>
        //[Header("预输入窗口 ")] public float preTime;

        /// <summary>
        /// 当前架势值
        /// </summary>
        [HideInInspector] public int currentSP = 0;
        /// <summary>
        /// 当前血瓶条
        /// </summary>
        [HideInInspector] public int currentCure = 0;
        /// <summary>
        /// 遭受伤害
        /// </summary>
        [HideInInspector] public int damageToTake = 0;
        /// <summary>
        /// 攻击连段计时
        /// </summary>
        [HideInInspector] public float attackTimer = 0;
        /// <summary>
        /// 闪避冷却计时
        /// </summary>
        [HideInInspector] public float dashColdTimer = 0;
        /// <summary>
        /// 是否处于极限闪避窗口期(前)
        /// </summary>
        [HideInInspector] public bool inDashWindow = false;
        /// <summary>
        /// 是否处于极限闪避窗口期(后)
        /// </summary>
        [HideInInspector] public bool inDashWindow_back = false;
        /// <summary>
        /// 是否落地
        /// </summary>
        [HideInInspector] public bool hasFall = false;
        /// <summary>
        /// 是否处于被攻击状态
        /// </summary>
        [HideInInspector] public bool isAttacked = false;
        /// <summary>
        /// 当前状态可否取消(只用于闪避攻击等)
        /// </summary>
        [HideInInspector] public bool canCancel = true;
        /// <summary>
        /// 是否处于霸体状态
        /// </summary>
        [HideInInspector] public bool isUnstoppable = false;
        /// <summary>
        /// 角色动画机
        /// </summary>
        [HideInInspector] public Animator animator;
        /// <summary>
        /// 角色刚体
        /// </summary>
        [HideInInspector] public Rigidbody2D myRigidBody;

        /// <summary>
        /// 攻击判定框normal_1
        /// </summary>
        [HideInInspector] public GameObject attackRange_normal_1;
        /// <summary>
        /// 攻击判定框normal_2
        /// </summary>
        [HideInInspector] public GameObject attackRange_normal_2;
        /// <summary>
        /// 攻击判定框normal_3
        /// </summary>
        [HideInInspector] public GameObject attackRange_normal_3;

        /// <summary>
        /// 角色状态机
        /// </summary>
        private FSM.StateMachine fsm;

        void Start()
        {
            //数据成员初始化
            faceDir = 1;

            groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();
            myRigidBody = GetComponent<Rigidbody2D>();

            fsm = new FSM.StateMachine(gameObject);
            fsm.OnEnable();

            animator = GetComponent<Animator>();

            attackRange_normal_1 = transform.Find("AttackRange_normal_1").gameObject;
            attackRange_normal_2 = transform.Find("AttackRange_normal_2").gameObject;
            attackRange_normal_3 = transform.Find("AttackRange_normal_3").gameObject;
        }


        void Update()
        {
            fsm.OnUpdate();

            GetGroundState();
            AttackTimeCount();
            DashColdDown();

            if(Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log(fsm.CurrentState);
            }
        }

        void FixedUpdate()
        {
            fsm.OnFixedUpdate();
        }

        /// <summary>
        /// 水平移动输入
        /// </summary>
        public void MoveHorizontal()
        {
            //获取水平移动输入,修改朝向和速度
            float inputX = Input.GetAxis("Horizontal");

            if (inputX > 0)
            {
                faceDir = 1;
            }
            else if (inputX < 0)
            {
                faceDir = -1;
            }

            transform.localScale = new Vector3(faceDir, 1, 1);
            myRigidBody.velocity = new Vector2(inputX * moveSpeed, myRigidBody.velocity.y);
        }

        /// <summary>
        /// 攻击连段计时
        /// </summary>
        public void AttackTimeCount()
        {
            if(attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
        }

        /// <summary>
        /// 闪避冷却
        /// </summary>
        public void DashColdDown()
        {       
            if(dashColdTimer > 0)
            {
                dashColdTimer -= Time.deltaTime;
            }
        }

        /// <summary>
        /// 受伤
        /// </summary>
        /// <param name="damage"></param>
        public void Hurt(int damage)
        {
            currentHP -= damage;

            if(currentHP < 0)
            {
                //死亡
                End_state_now();
                fsm.SetState(FSM.StateKind.Dead);
                fsm.CurrentState.OnEnter();
            }
            else if(!isUnstoppable)
            {
                //受击
                End_state_now();
                fsm.SetState(FSM.StateKind.Hurt);
                fsm.CurrentState.OnEnter();
            }

            
        }

        /// <summary>
        /// 极限闪避
        /// </summary>
        public void UltimateEvasion(bool ahead)
        {
            if(ahead)
            {
                int _currentSP = currentSP + dashGain_SP;
                currentSP = (int)MathF.Min(_currentSP, maxSP);
            }
            else
            {
                int _currentCure = currentCure + dashGain_Cure;
                currentCure = (int)MathF.Min(_currentCure, maxCure);
            }
        }

        #region 用于帧事件调用(其中int参数使用1/0表示true/false)
        /// <summary>
        /// 播放移动动画
        /// </summary>
        public void Play_move()
        {
            animator.Play("Move_player");
        }

        /// <summary>
        /// 终止当前状态
        /// </summary>
        public void End_state_now()
        {
            fsm.CurrentState.OnExit();
        }
        /// <summary>
        /// 设置是否为极限闪避窗口期
        /// </summary>
        /// <param name="isTrue"></param>
        public void Set_inDashWindow(int isTrue)
        {
            inDashWindow = Convert.ToBoolean(isTrue);
        }
        /// <summary>
        /// 设置状态可否取消
        /// </summary>
        /// <param name="isTrue"></param>
        public void Set_canCancel(int isTrue)
        {
            canCancel = Convert.ToBoolean(isTrue);
            Debug.Log("canCancel:" + Convert.ToBoolean(isTrue));
        }
        /// <summary>
        /// 设置霸体状态
        /// </summary>
        /// <param name="isTrue"></param>
        public void Set_isUnstoppable(int isTrue)
        {
            isUnstoppable = Convert.ToBoolean(isTrue);
        }
        /// <summary>
        /// 设置攻击框normal_1
        /// </summary>
        /// <param name="isActive"></param>
        public void Set_normal_1(int isActive)
        {
            attackRange_normal_1.SetActive(Convert.ToBoolean(isActive));
        }
        public void Set_normal_2(int isActive)
        {
            attackRange_normal_2.SetActive(Convert.ToBoolean(isActive));
        }
        public void Set_normal_3(int isActive)
        {
            attackRange_normal_3.SetActive(Convert.ToBoolean(isActive));
        }
        #endregion
    }
}
