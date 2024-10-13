using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        /// <summary>
        /// 闪避时间
        /// </summary>
        [Header("闪避时间")] public float dashTime;
        /// <summary>
        /// 闪避窗口期
        /// </summary>
        [Header("闪避窗口期")] public float dashWindow;
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
        /// 
        /// </summary>
        [HideInInspector] public GameObject attackRange;

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

            if (animator != null )
            {
                Debug.Log("Find Animator");
            }
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
    }
}
