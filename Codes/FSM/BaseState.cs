using UnityEngine;

namespace FSM
{
    /// <summary>
    /// 状态基类
    /// </summary>
    public abstract class BaseState
    {
        protected StateMachine myFSM;
        protected GameObject myObject;
        public BaseState(StateMachine fsm)
        {
            myFSM = fsm;
            myObject = fsm.myObject;
        }
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
        public abstract void OnExit();
    }

    /// <summary>
    /// 状态种类枚举
    /// </summary>
    public enum StateKind
    {
        Idle,
        Move,
        Jump,
        Attack,
        Dash,
        Hurt
    }
}
