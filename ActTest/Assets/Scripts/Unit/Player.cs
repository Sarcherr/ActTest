using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        /// 最大架势值
        /// </summary>
        [Header("最大架势值")] public int maxSP;
        /// <summary>
        /// 预输入窗口    
        /// </summary>
        [Header("预输入窗口 ")] public float preTime;

        /// <summary>
        /// 当前架势值
        /// </summary>
        [HideInInspector] public int currentSP;
        /// <summary>
        /// 闪避冷却计时
        /// </summary>
        [HideInInspector] public float dashColdTimer;
        /// <summary>
        /// 是否处于极限闪避窗口期
        /// </summary>
        [HideInInspector] public bool inDashWindow;
        /// <summary>
        /// 是否处于被攻击状态
        /// </summary>
        [HideInInspector] public bool isAttacked;
        /// <summary>
        /// 当前状态可否取消(只用于闪避攻击等)
        /// </summary>
        [HideInInspector] public bool canCancel;
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
            currentSP = 0;
            isGrounded = true;
            inDashWindow = false;

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
            DashColdDown();
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
        /// 闪避冷却
        /// </summary>
        public void DashColdDown()
        {       
            if(dashColdTimer > 0)
            {
                dashColdTimer -= Time.deltaTime;
            }
        }

        #region 用于帧事件调用(其中int参数使用1/0表示true/false)
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
