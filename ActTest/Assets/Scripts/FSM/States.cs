 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class IdleState : BaseState
    {
        public Rigidbody2D myRigidBody;

        public IdleState(StateMachine fsm):base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            Debug.Log("Idle Enter");

            myPlayer.animator.SetBool("isIdle", true);
            //�������������ֹ��ɫͣ��ʱ����̫Զ
            myRigidBody.drag = 1;
        }

        public override void OnExit()
        {
            Debug.Log("Idle Exit");

            myPlayer.animator.SetBool("isIdle", false);
            myRigidBody.drag = 0;
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {
            //���Ŷ���

            if(Input.GetKeyDown(KeyCode.LeftShift) && myPlayer.dashColdTimer <= 0)//����
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
            else if(Input.GetMouseButtonDown(1))//�ṥ��
            {
                myFSM.SetState(StateKind.Attack_normal);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if(Input.GetKeyDown(KeyCode.Space))//��Ծ
            {
                myFSM.SetState(StateKind.Jump);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }   
            else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || !myPlayer.isGrounded)//�ƶ�
            {
                myFSM.SetState(StateKind.Move);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
        }
    }

    public class MoveState : BaseState
    {
        public Rigidbody2D myRigidBody;

        private float speedY;

        public MoveState(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            Debug.Log("Move Enter");

            if (!myPlayer.isGrounded && myRigidBody.velocity.y < 0)
            {
                myPlayer.animator.SetBool("isFall", true);
            }
            else
            {
                myPlayer.animator.SetBool("isMove", true);
            }
        }

        public override void OnExit()
        {
            //����,��ֹ���ʱOnUpdateδ�ָ�����
            myRigidBody.gravityScale = 1;
            Debug.Log("Move Exit");

            myPlayer.animator.SetBool("isFall", false);
            myPlayer.animator.SetBool("isMove", false);
        }

        public override void OnFixedUpdate()
        {
            //����ˮƽ�ƶ�����
            myPlayer.MoveHorizontal();

            //��׹ͬ��������Move״̬��
            if (!myPlayer.isGrounded && myRigidBody.velocity.y <= 0)
            {
                myPlayer.animator.SetBool("isFall", true);
                myPlayer.animator.SetBool("isMove", false);
                //��׹ʱ��������
                myRigidBody.gravityScale = myPlayer.fallGravity;
                //���������׹�ٶ�
                speedY = Mathf.Min(myRigidBody.velocity.y, myPlayer.maxFallSpeed);
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, speedY);
            }
            else
            {
                myPlayer.animator.SetBool("isFall", false);
                myPlayer.animator.SetBool("isMove", true);
                myRigidBody.gravityScale = 1;
            }
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && myPlayer.dashColdTimer <= 0)//����
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
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
            else if (Input.GetKeyDown(KeyCode.Space) && myPlayer.isGrounded)//��Ծ
            {
                myFSM.SetState(StateKind.Jump);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if(!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)//�ɿ�AD��δ��׹��
            {
                myFSM.SetState(StateKind.Idle);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
        }
    }

    public class JumpState : BaseState
    {
        public Rigidbody2D myRigidBody;
         
        public JumpState(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myPlayer.myRigidBody;
        }
        public override void OnEnter()
        {
            Debug.Log("Jump Enter");

            //��ȡ��ֱ�����ٶ�
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, myPlayer.jumpForce);
            //��ʱ����groundsensor������Ծ����isgrounded��Ϊtrue
            myPlayer.groundSensor.Disable(0.2f);
            myPlayer.animator.SetBool("isJump", true);
        }

        public override void OnExit()
        {
            Debug.Log("Jump Exit");
            myPlayer.animator.SetBool("isJump", false);
        }

        public override void OnFixedUpdate()
        {
            //����ˮƽ�ƶ�����
            myPlayer.MoveHorizontal();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && myPlayer.dashColdTimer <= 0)//����
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
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
            else if (myRigidBody.velocity.y <= 0)//��׹ʱ�л���������׹��Move״̬
            {
                myFSM.SetState(StateKind.Move);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
        }
    }

    public class AttackState : BaseState
    {
        public int attackDamage;

        public AttackState(StateMachine fsm) : base(fsm)
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

    public class AttackState_normal : BaseState
    {
        public Rigidbody2D myRigidBody;
        //�����˺�
        public int attackDamage;
        //�����ƶ�����
        public float attackMoveForce = 1.5f;
        //����������
        public int attackNum = 0;

        private float attackTimer;

        public AttackState_normal(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            Debug.Log("Attack_normal_1 Enter");
            myPlayer.animator.SetBool("isAttack_normal_1", true);
            myRigidBody.AddForce(new Vector2(myPlayer.faceDir * attackMoveForce, 0), ForceMode2D.Impulse);
        }

        public override void OnExit()
        {
            if (myPlayer.isGrounded)
            {
                myFSM.SetState(StateKind.Idle);
            }
            else
            {
                myFSM.SetState(StateKind.Move);
            }
            myFSM.CurrentState.OnEnter();
            myPlayer.animator.SetBool("isAttack_normal_1", false);
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {

        }
    }

    public class AttackState_heavy : BaseState
    {
        public int attackDamage;

        public AttackState_heavy(StateMachine fsm) : base(fsm)
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

    public class AttackState_skill_1 : BaseState
    {
        public int attackDamage;

        public AttackState_skill_1(StateMachine fsm) : base(fsm)
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

    public class AttackState_skill_2 : BaseState
    {
        public int attackDamage;

        public AttackState_skill_2(StateMachine fsm) : base(fsm)
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

    public class DashState : BaseState
    {
        private Rigidbody2D myRigidBody;
        //private float timer;

        public DashState(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myObject.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            //����ͨ������̶�����ٶȺ�ȡ������ʵ��
            Debug.Log("Dash Enter");
            //timer = 0;
            myRigidBody.gravityScale = 0;
            myRigidBody.velocity = new Vector2 (myPlayer.dashSpeed * myPlayer.faceDir, 0);

            myPlayer.animator.SetBool("isDash", true);
        }

        public override void OnExit()
        {
            Debug.Log("Dash Exit");
            myRigidBody.gravityScale = 1;
            myRigidBody.velocity = new Vector2(0, 0);
            //timer = 0;
            myPlayer.dashColdTimer = myPlayer.dashCold;

            if (myPlayer.isGrounded)
            {
                myFSM.SetState(StateKind.Idle);
            }
            else
            {
                myFSM.SetState(StateKind.Move);
            }
            myFSM.CurrentState.OnEnter();
            myPlayer.animator.SetBool("isDash", false);
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnUpdate()
        {
            //timer += Time.deltaTime;
            //if(timer > myPlayer.dashWindow && timer <= myPlayer.dashTime)
            //{
            //    //�������ܴ������ж�
            //    myPlayer.inDashWindow = false;
            //}
            //else if(timer > myPlayer.dashTime)
            //{
            //    myFSM.SetState(StateKind.Idle);
            //    OnExit();
            //    myFSM.CurrentState.OnEnter();  
            //}
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

    public class DefaultState : BaseState
    {
        public DefaultState(StateMachine fsm) : base(fsm)
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
