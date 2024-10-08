using UnityEngine;

namespace FSM
{
    /// <summary>
    /// ״̬����
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
    /// ״̬����ö��
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