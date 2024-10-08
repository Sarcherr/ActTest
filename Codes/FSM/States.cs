 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class IdleState : BaseState
    {
        public IdleState(StateMachine fsm):base(fsm)
        {

        }
        public override void OnEnter()
        {
            Debug.Log("Idle Enter");
        }

        public override void OnExit()
        {
            Debug.Log("Idle Exit");
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {
            //���Ŷ���

            if(Input.GetKeyDown(KeyCode.LeftShift))//
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if(false)//���ܹ���
            {
                
            }
            else if(false)//�ع���
            {

            }
            else if(false)//�ṥ��
            {

            }
            else if(Input.GetKeyDown(KeyCode.Space))//��Ծ
            {

            }
            else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))//�ƶ�
            {

            }
        }
    }

    public class MoveState : BaseState
    {
        public MoveState(StateMachine fsm) : base(fsm)
        {

        }
        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
            
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnUpdate()
        {
            
        }
    }

    public class JumpState : BaseState
    {
        public JumpState(StateMachine fsm) : base(fsm)
        {

        }
        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {

        }
    }

    public class AttackState : BaseState
    {
        public AttackState(StateMachine fsm) : base(fsm)
        {

        }
        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public override void OnFixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }

    public class DashState : BaseState
    {
        private Unit.Player myPlayer;
        private Rigidbody2D myRigidBody;
        private float timer;

        public DashState(StateMachine fsm) : base(fsm)
        {
            myPlayer = myObject.GetComponent<Unit.Player>();
            myRigidBody = myObject.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            //����ͨ������̶�����ٶȺ�ȡ������ʵ��
            Debug.Log("Dash Enter");
            timer = 0;
            myPlayer.inDashWindow = true;
            myRigidBody.gravityScale = 0;
            myRigidBody.velocity = new Vector2 (myPlayer.dashSpeed * myPlayer.faceDir, 0);
        }

        public override void OnExit()
        {
            Debug.Log("Dash Exit");
            myObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            myPlayer.inDashWindow = false;
            timer = 0;
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnUpdate()
        {
            timer += Time.deltaTime;
            if(timer > myPlayer.dashWindow && timer <= myPlayer.dashTime)
            {
                //�������ܴ������ж�
                myPlayer.inDashWindow = false;
            }
            else if(timer > myPlayer.dashTime)
            {
                //TODO:��Ӽ�⣬���ܽ������ڿ�����׹�䣬�ڵ����д���
                myFSM.SetState(StateKind.Idle);
                OnExit();
                myFSM.CurrentState.OnEnter();  
            }
        }
    }

    public class HurtState : BaseState
    {
        public HurtState(StateMachine fsm) : base(fsm)
        {

        }
        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public override void OnFixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
