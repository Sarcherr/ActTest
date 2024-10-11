using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    //继承的数据成员:
    //(最大生命值)int maxHP;
    //(移动速度)  float moveSpeed;
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
        /// 跳跃力度
        /// </summary>
        [Header("跳跃力度")] public float jumpForce;
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
        /// 是否处于极限闪避窗口期
        /// </summary>
        [HideInInspector] public bool inDashWindow;

        /// <summary>
        /// 角色刚体
        /// </summary>
        private Rigidbody2D myRigidBody;
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
        }


        void Update()
        {
            fsm.OnUpdate();
            GetGroundState();
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
    }
}