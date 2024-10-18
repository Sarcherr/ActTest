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

            myPlayer.animator.Play("Idle_player");
            //�������������ֹ��ɫͣ��ʱ����̫Զ
            myRigidBody.drag = 1;
        }

        public override void OnExit()
        {
            Debug.Log("Idle Exit");

            //myPlayer.animator.SetBool("isIdle", false);
            myRigidBody.drag = 0;
        }

        public override void OnFixedUpdate()
        {

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
            else if (Input.GetMouseButtonDown(0))//�ṥ��
            {
                myFSM.SetState(StateKind.Attack_normal);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.Space))//��Ծ
            {
                myFSM.SetState(StateKind.Jump);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            if (myFSM.PreState != StateKind.Default)
            {
                myFSM.SetState(myFSM.PreState);
                OnExit();
                myFSM.CurrentState.OnEnter();
                myFSM.PreState = StateKind.Default;
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
                myPlayer.animator.Play("Fall_player");
            }
            else
            {
                myPlayer.animator.Play("Move_player");
            }
        }

        public override void OnExit()
        {
            //����,��ֹ���ʱOnUpdateδ�ָ�����
            myRigidBody.gravityScale = 1;
            Debug.Log("Move Exit");

            //myPlayer.animator.SetBool("isFall", false);
            //myPlayer.animator.SetBool("isMove", false);
        }

        public override void OnFixedUpdate()
        {
            //����ˮƽ�ƶ�����
            myPlayer.MoveHorizontal();

            //��׹ͬ��������Move״̬��
            if (!myPlayer.isGrounded && myRigidBody.velocity.y <= 0)
            {
                myPlayer.animator.Play("Fall_player");
                //myPlayer.animator.SetBool("isMove", false);
                //��׹ʱ��������
                myRigidBody.gravityScale = myPlayer.fallGravity;
                //���������׹�ٶ�
                speedY = Mathf.Min(myRigidBody.velocity.y, myPlayer.maxFallSpeed);
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, speedY);
            }
            else
            {
                //myPlayer.animator.SetBool("isFall", false);
                myPlayer.animator.Play("Move_player");
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
            else if (Input.GetMouseButtonDown(0))//�ṥ��
            {
                myFSM.SetState(StateKind.Attack_normal);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && myPlayer.isGrounded)//��Ծ
            {
                myFSM.SetState(StateKind.Jump);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            if (myFSM.PreState != StateKind.Default)
            {
                myFSM.SetState(myFSM.PreState);
                OnExit();
                myFSM.CurrentState.OnEnter();
                myFSM.PreState = StateKind.Default;
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
            myPlayer.animator.Play("Jump_player");
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
            else if (Input.GetMouseButtonDown(0))//�ṥ��
            {
                myFSM.SetState(StateKind.Attack_normal);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (myRigidBody.velocity.y <= 0)//��׹ʱ�л���������׹��Move״̬
            {
                myFSM.SetState(StateKind.Move);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
        }
    }

    public class AttackState_normal : BaseState
    {
        public Rigidbody2D myRigidBody;
        //�����˺�
        public int attackDamage;
        //�����ƶ�����
        public float attackMoveForce;
        //����������
        public int attackNum = 0;
        //�Ƿ�ȡ��
        public bool isCancel = false;

        public AttackState_normal(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
            attackDamage = myPlayer.attackDamage_normal;
            attackMoveForce = myPlayer.attackMoveForce_normal;
        }
        public override void OnEnter()
        {
            Debug.Log("Attack_normal Enter");
            isCancel = false;
            if (attackNum < 3 && myPlayer.attackTimer > 0)
            {
                attackNum++;
            }
            else
            {
                attackNum = 1;
            }

            //ʹ�����ͣ��(Maybe)
            myRigidBody.velocity = Vector2.zero;
            switch (attackNum)
            {
                case 1:
                    myPlayer.animator.Play("Attack_normal_1_player");
                    //myPlayer.animator.SetBool("isAttack_normal_1", true);
                    break;
                case 2:
                    myPlayer.animator.Play("Attack_normal_2_player");
                    break;    
                case 3:
                    myPlayer.animator.Play("Attack_normal_3_player");
                    break;
            }
            Debug.Log(attackNum);
            myRigidBody.AddForce(new Vector2(myPlayer.faceDir * attackMoveForce, 0), ForceMode2D.Impulse);
        }

        public override void OnExit()
        {
            Debug.Log("Attack_normal Exit");

            myPlayer.attackTimer = myPlayer.attackTime;
            myPlayer.Set_normal_1(0);
            myPlayer.Set_normal_2(0);
            myPlayer.Set_normal_3(0);

            if(!isCancel)
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
            }
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {
            //ȡ��ǰ��ҡ
            //if(myPlayer.canCancel)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && myPlayer.dashColdTimer <= 0)//����
                {
                    isCancel = true;
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
            }
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
            myRigidBody.gravityScale = 0;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                Debug.Log("Dash_ahead Enter");
                //timer = 0;               
                myRigidBody.velocity = new Vector2(myPlayer.dashSpeed * myPlayer.faceDir, 0);
                myPlayer.animator.Play("Dash_player");
            }
            else
            {
                Debug.Log("Dash_back Enter");
                //timer = 0;               
                myRigidBody.velocity = new Vector2(-myPlayer.dashSpeed * myPlayer.faceDir, 0);
                myPlayer.animator.Play("Dash_player");
            }
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
            //myPlayer.animator.SetBool("isDash", false);
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
