using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    /// <summary>
    /// 玩家角色类
    /// </summary>
    public class Player : Unit
    {
        [Header("跳跃力度")] public float jumpForce;
        [Header("闪避速度")] public float dashSpeed;
        [Header("闪避时间")] public float dashTime;
        [Header("闪避窗口期")] public float dashWindow;
        [Header("最大架势值")] public int maxSP;
       
        [HideInInspector] public int currentSP;
        [HideInInspector] public bool inDashWindow;

        private FSM.StateMachine fsm;

        void Start()
        {
            //数据成员初始化
            faceDir = 1;
            currentSP = 0;
            isGrounded = true;
            inDashWindow = false;

            groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();

            fsm = new FSM.StateMachine(gameObject);
            fsm.OnEnable();
        }


        void Update()
        {
            fsm.OnUpdate();
        }

        void FixedUpdate()
        {
            fsm.OnFixedUpdate();
        }
    }
}