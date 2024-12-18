using System.Collections;
using System.Collections.Generic;
using Unit;
using Unity.VisualScripting;
using UnityEngine;

namespace FSM
{
    public class StateMachine
    {
        /// <summary>
        /// 绑定游戏对象
        /// </summary>
        public GameObject myObject;
        /// <summary>
        /// 当前状态
        /// </summary>
        public BaseState CurrentState;
        /// <summary>
        /// 当前状态类型
        /// </summary>
        public StateKind CurrentStateKind;
        /// <summary>
        /// 状态表
        /// </summary>
        public Dictionary<StateKind, BaseState> StateMap;

        /// <summary>
        /// 构造函数(参数为FSM绑定的游戏对象)
        /// </summary>
        /// <param name="m_object"></param>
        public StateMachine(GameObject m_object)
        {
            myObject = m_object;
            StateMap = new Dictionary<StateKind, BaseState>();
        }
        /// <summary>
        /// 设置状态(参数使用StateKind枚举)
        /// </summary>
        /// <param name="stateKind"></param>
        public void SetState(StateKind stateKind)
        {
            //状态表中存在指定状态且currentState不为指定状态时修改状态
            if(StateMap.ContainsKey(stateKind))
            {
                if(CurrentState != StateMap[stateKind])
                {
                    CurrentState = StateMap[stateKind];
                    CurrentStateKind = stateKind;
                }
            }
            else
            {
                Debug.Log("状态不存在");
            }
        }
        /// <summary>
        /// 添加状态(参数使用StateKind,BaseState(注意new))
        /// </summary>
        /// <param name="stateKind"></param>
        public void AddState(StateKind stateKind, BaseState baseState)
        {
            //状态表中不存在指定状态时添加状态
            if (!StateMap.ContainsKey(stateKind))
            {
                StateMap.Add(stateKind, baseState);
            }
        }
        /// <summary>
        /// 状态机初始化(后续可以有依据绑定对象的分支)
        /// </summary>
        public void OnEnable()
        {
            Debug.Log("初始化开始");
            //初始化添加玩家角色具有的状态
            AddState(StateKind.Idle, new IdleState(this));
            AddState(StateKind.Move, new MoveState(this));
            AddState(StateKind.Fall, new FallState(this));
            AddState(StateKind.Jump, new JumpState(this));
            AddState(StateKind.Attack_normal, new AttackState_normal(this));
            AddState(StateKind.Attack_sky, new AttackState_sky(this));
            AddState(StateKind.Attack_heavy, new AttackState_heavy(this));
            AddState(StateKind.Attack_skill_1, new AttackState_skill_1(this));
            AddState(StateKind.Attack_skill_2, new AttackState_skill_2(this));
            AddState(StateKind.Dash, new DashState(this));
            AddState(StateKind.Dash_extreme, new DashState_extreme(this));
            AddState(StateKind.Hurt, new HurtState(this));
            AddState(StateKind.Dead, new DeadState(this));
            //初始设置待机状态
            SetState(StateKind.Idle);
            CurrentState.OnEnter();

            Debug.Log("初始化成功");
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
