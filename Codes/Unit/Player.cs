using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    //�̳е����ݳ�Ա:
    //(�������ֵ)int maxHP;
    //(�ƶ��ٶ�)  float moveSpeed;
    //(����)      int faceDir;
    //(��ǰ����ֵ)int currentHP;
    //(�Ƿ񴥵�)  bool isGrounded;
    //(���ش�����)GroundSensor groundSensor;
    /// <summary>
    /// ��ҽ�ɫ��
    /// </summary>
    public class Player : Unit
    {
        /// <summary>
        /// ��Ծ����
        /// </summary>
        [Header("��Ծ����")] public float jumpForce;
        /// <summary>
        /// �����ٶ�
        /// </summary>
        [Header("�����ٶ�")] public float dashSpeed;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        [Header("����ʱ��")] public float dashTime;
        /// <summary>
        /// ���ܴ�����
        /// </summary>
        [Header("���ܴ�����")] public float dashWindow;
        /// <summary>
        /// ������ȴ    
        /// </summary>
        [Header("������ȴ")] public float dashCold;
        /// <summary>
        /// ������ֵ
        /// </summary>
        [Header("������ֵ")] public int maxSP;
        /// <summary>
        /// Ԥ���봰��    
        /// </summary>
        [Header("Ԥ���봰�� ")] public float preTime;

        /// <summary>
        /// ��ǰ����ֵ
        /// </summary>
        [HideInInspector] public int currentSP;
        /// <summary>
        /// �Ƿ��ڼ������ܴ�����
        /// </summary>
        [HideInInspector] public bool inDashWindow;

        /// <summary>
        /// ��ɫ����
        /// </summary>
        private Rigidbody2D myRigidBody;
        /// <summary>
        /// ��ɫ״̬��
        /// </summary>
        private FSM.StateMachine fsm;

        void Start()
        {
            //���ݳ�Ա��ʼ��
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
        /// ˮƽ�ƶ�����
        /// </summary>
        public void MoveHorizontal()
        {
            //��ȡˮƽ�ƶ�����,�޸ĳ�����ٶ�
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