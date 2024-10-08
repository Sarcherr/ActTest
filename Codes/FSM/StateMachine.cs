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
        /// ״̬��
        /// </summary>
        public Dictionary<StateKind, BaseState> StateMap;

        /// <summary>
        /// ���캯��(����ΪFSM�󶨵���Ϸ����)
        /// </summary>
        /// <param name="m_object"></param>
        public StateMachine(GameObject m_object)
        {
            myObject = m_object;
            StateMap = new Dictionary<StateKind, BaseState>();
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
            //��ʼ���ô���״̬
            SetState(StateKind.Idle);
            CurrentState.OnEnter();

            Debug.Log("��ʼ���ɹ�");
        }
        public void OnUpdate()
        {
            CurrentState.OnUpdate();
        }
        public void OnFixedUpdate()
        {
            CurrentState.OnFixedUpdate();
        }
    }
}