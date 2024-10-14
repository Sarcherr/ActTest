using System.Collections;
using System.Collections.Generic;
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
        /// 预输入指令
        /// </summary>
        public StateKind PreState;
        /// <summary>
        /// 状态表
        /// </summary>
        public Dictionary<StateKind, BaseState> StateMap;

        /// <summary>
        /// 预输入窗口
        /// </summary>
        private float preTime = 0.2f;
        /// <summary>
        /// 预输入计时器
        /// </summary>
        private float preTimer;

        /// <summary>
        /// 构造函数(参数为FSM绑定的游戏对象)
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
        /// 获取预输入指令
        /// </summary>
        public void GetPreState()
        {
            if (false)//技能攻击1
            {
                PreState = StateKind.Attack_skill_1;
            }
            else if (false)//技能攻击2
            {
                PreState = StateKind.Attack_skill_2;
            }
            else if (Input.GetMouseButtonDown(2))//重攻击
            {
                PreState = StateKind.Attack_heavy;
            }
            else if (Input.GetMouseButtonDown(1))//轻攻击
            {
                PreState = StateKind.Attack_normal;
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
        /// 状态机初始化(后续可以有依据绑定对象的分支)
        /// </summary>
        public void OnEnable()
        {
            Debug.Log("初始化开始");
            //初始化添加玩家角色具有的状态
            AddState(StateKind.Idle, new IdleState(this));
            AddState(StateKind.Move, new MoveState(this));
            AddState(StateKind.Jump, new JumpState(this));
            AddState(StateKind.Attack_normal, new AttackState_normal(this));
            AddState(StateKind.Attack_heavy, new AttackState_heavy(this));
            AddState(StateKind.Attack_skill_1, new AttackState_skill_1(this));
            AddState(StateKind.Attack_skill_2, new AttackState_skill_2(this));
            AddState(StateKind.Dash, new DashState(this));
            AddState(StateKind.Hurt, new HurtState(this));
            //初始设置待机状态
            SetState(StateKind.Idle);
            CurrentState.OnEnter();

            Debug.Log("初始化成功");
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
