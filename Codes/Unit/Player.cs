using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    /// <summary>
    /// ��ҽ�ɫ��
    /// </summary>
    public class Player : Unit
    {
        [Header("��Ծ����")] public float jumpForce;
        [Header("�����ٶ�")] public float dashSpeed;
        [Header("����ʱ��")] public float dashTime;
        [Header("���ܴ�����")] public float dashWindow;
        [Header("������ֵ")] public int maxSP;
       
        [HideInInspector] public int currentSP;
        [HideInInspector] public bool inDashWindow;

        private FSM.StateMachine fsm;

        void Start()
        {
            //���ݳ�Ա��ʼ��
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