using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FSM
{
    public class StateMachine
    {
        /// <summary>
        /// ����Ϸ����
        /// </summary>
        public GameObject myObject;
        /// <summary>
        /// ��ǰ״̬
        /// </summary>
        public BaseState CurrentState;
        /// <summary>
        /// Ԥ����ָ��
        /// </summary>
        public StateKind PreState;
        /// <summary>
        /// ״̬��
        /// </summary>
        public Dictionary<StateKind, BaseState> StateMap;

        /// <summary>
        /// �����ɷ�ȡ��
        /// </summary>
        private bool canCancel = true;
        /// <summary>
        /// Ԥ���봰��
        /// </summary>
        private float preTime = 0;
        /// <summary>
        /// Ԥ�����ʱ��
        /// </summary>
        private float preTimer;

        /// <summary>
        /// ���캯��(����ΪFSM�󶨵���Ϸ����)
        /// </summary>
        /// <param name="m_object"></param>
        public StateMachine(GameObject m_object)
        {
            myObject = m_object;
            StateMap = new Dictionary<StateKind, BaseState>();
            preTime = myObject.GetComponent<Unit.Player>().preTime;
            PreState = StateKind.Default;
        }
        /// <summary>
        /// ����״̬(����ʹ��StateKindö��)
        /// </summary>
        /// <param name="stateKind"></param>
        public void SetState(StateKind stateKind)
        {
            //״̬���д���ָ��״̬��currentState��Ϊָ��״̬ʱ�޸�״̬
            if(StateMap.ContainsKey(stateKind))
            {
                if(CurrentState != StateMap[stateKind])
                {
                    CurrentState = StateMap[stateKind];
                }
            }
            else
            {
                Debug.Log("״̬������");
            }
        }
        /// <summary>
        /// ���״̬(����ʹ��StateKind,BaseState(ע��new))
        /// </summary>
        /// <param name="stateKind"></param>
        public void AddState(StateKind stateKind, BaseState baseState)
        {
            //״̬���в�����ָ��״̬ʱ���״̬
            if (!StateMap.ContainsKey(stateKind))
            {
                StateMap.Add(stateKind, baseState);
            }
        }
        /// <summary>
        /// ��ȡԤ����ָ��
        /// </summary>
        public void GetPreState()
        {
            if (Input.GetKey(KeyCode.LeftShift))//����
            {
                PreState = StateKind.Dash;
            }
            else if (false)//���ܹ���
            {

            }
            else if (false)//�ع���
            {

            }
            else if (false)//�ṥ��
            {

            }
            else if (Input.GetKeyDown(KeyCode.Space))//��Ծ
            {
                PreState = StateKind.Jump;
            }

            if (PreState != StateKind.Default)
            {
                preTimer += Time.deltaTime;

                if(preTimer > preTime)
                {
                    preTimer = 0;
                    PreState = StateKind.Default;
                }
            }
        }
        /// <summary>
        /// ״̬����ʼ��(�������������ݰ󶨶���ķ�֧)
        /// </summary>
        public void OnEnable()
        {
            Debug.Log("��ʼ����ʼ");
            //��ʼ�������ҽ�ɫ���е�״̬
            AddState(StateKind.Idle, new IdleState(this));
            AddState(StateKind.Move, new MoveState(this));
            AddState(StateKind.Jump, new JumpState(this));
            AddState(StateKind.Attack, new AttackState(this));
            AddState(StateKind.Dash, new DashState(this));
            AddState(StateKind.Hurt, new HurtState(this));
            AddState(StateKind.Default, new DefaultState(this));
            //��ʼ���ô���״̬
            SetState(StateKind.Idle);
            CurrentState.OnEnter();

            Debug.Log("��ʼ���ɹ�");
        }
        public void OnUpdate()
        {
            CurrentState.OnUpdate();
            GetPreState();
        }
        public void OnFixedUpdate()
        {
            CurrentState.OnFixedUpdate();
        }
    }
}