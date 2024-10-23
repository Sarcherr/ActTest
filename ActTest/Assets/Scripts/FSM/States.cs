 using System.Collections;
using System.Collections.Generic;
using Unit;
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
            //�������������ֹ��ɫͣ��ʱ����̫Զ
            myRigidBody.drag = 2;

            if (myPlayer.hasFall)
            {
                myPlayer.animator.Play("Fall_end_player");
                myPlayer.Set_canCancel(0);
            }
            else
            {
                myPlayer.animator.Play("Idle_player");
                myPlayer.Set_canCancel(1);
            }

            myPlayer.hasFall = false;
        }

        public override void OnExit()
        {
            myRigidBody.drag = 0;
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {
            if(myPlayer.canCancel)
            {
                myPlayer.animator.Play("Idle_player");
            }

            if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//����
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (!myPlayer.isGrounded)//��׹
            {
                myFSM.SetState(StateKind.Fall);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.I) && myPlayer.isGrounded)//���ܹ���
            {

            }
            else if (Input.GetKeyDown(KeyCode.J) && myPlayer.isGrounded)//�ṥ��
            {
                myFSM.SetState(StateKind.Attack_normal);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.K) && myPlayer.isGrounded)//��Ծ
            {
                myFSM.SetState(StateKind.Jump);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && myPlayer.isGrounded)//�ƶ�
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

        public MoveState(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            if(!myPlayer.hasFall)
            {
                myPlayer.animator.Play("Move_ready_player");
                myPlayer.Set_canCancel(0);
            }
            else
            {
                myPlayer.Set_canCancel(1);
            }

            myPlayer.hasFall = false;
        }

        public override void OnExit()
        {
        }

        public override void OnFixedUpdate()
        {
            //����ˮƽ�ƶ�����
            myPlayer.MoveHorizontal();

            if (myPlayer.canCancel)
            {
                myPlayer.animator.Play("Move_player");  
                myRigidBody.gravityScale = 1;
            }
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//����
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (!myPlayer.isGrounded)//��׹
            {
                myFSM.SetState(StateKind.Fall);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.I) && myPlayer.isGrounded)//���ܹ���
            {

            }
            else if (Input.GetKeyDown(KeyCode.J) && myPlayer.isGrounded)//�ṥ��
            {
                myFSM.SetState(StateKind.Attack_normal);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.K) && myPlayer.isGrounded)//��Ծ
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

    public class FallState : BaseState
    {
        public Rigidbody2D myRigidBody;
        private float speedY;

        public FallState(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            myPlayer.animator.Play("Fall_player");
            //��׹ʱ��������
            myRigidBody.gravityScale = myPlayer.fallGravity;
        }

        public override void OnExit()
        {
            myRigidBody.gravityScale = 1;
        }

        public override void OnFixedUpdate()
        {
            //����ˮƽ�ƶ�����
            myPlayer.MoveHorizontal();
            //���������׹�ٶ�
            speedY = Mathf.Min(myRigidBody.velocity.y, myPlayer.maxFallSpeed);
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, speedY);
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//����
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.J))//���й���
            {
                //myFSM.SetState(StateKind.Attack_sky);
                //OnExit();
                //myFSM.CurrentState.OnEnter();
            }
            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)//�ƶ�
            {
                myPlayer.hasFall = true;
                myFSM.SetState(StateKind.Move);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)//�ɿ�AD��δ��׹��
            {
                myPlayer.hasFall = true;
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
            //��ȡ��ֱ�����ٶ�
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, myPlayer.jumpForce);
            //��ʱ����groundsensor������Ծ����isgrounded��Ϊtrue
            myPlayer.groundSensor.Disable(0.2f);
            myPlayer.animator.Play("Jump_player");
        }

        public override void OnExit()
        {
            myPlayer.animator.SetBool("isJump", false);
        }

        public override void OnFixedUpdate()
        {
            //����ˮƽ�ƶ�����
            myPlayer.MoveHorizontal();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//����
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.J))//���й���
            {
                //myFSM.SetState(StateKind.Attack_sky);
                //OnExit();
                //myFSM.CurrentState.OnEnter();
            }
            else if (myRigidBody.velocity.y <= 0)//��׹ʱ�л���׹״̬
            {
                myFSM.SetState(StateKind.Fall);
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

        //Ԥ���봰��
        public float preTime = 0.2f;
        //Ԥ�����ʱ��
        public float preTimer = 0;

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
            preTimer = 0;

            if (Input.GetKey(KeyCode.J))
            {
                myPlayer.hasPull = true;
            }
            else
            {
                myPlayer.hasPull = false;
            }

            if (attackNum < 4 && myPlayer.attackTimer > 0)
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
                    myRigidBody.AddForce(new Vector2(myPlayer.faceDir * attackMoveForce, 0), ForceMode2D.Impulse);
                    break;
                case 2:
                    myPlayer.animator.Play("Attack_normal_2_player");
                    myRigidBody.AddForce(new Vector2(myPlayer.faceDir * attackMoveForce, 0), ForceMode2D.Impulse);
                    break;    
                case 3:
                    myPlayer.animator.Play("Attack_normal_3_player");
                    myRigidBody.AddForce(new Vector2(myPlayer.faceDir * attackMoveForce, 0), ForceMode2D.Impulse);
                    break;
                case 4:
                    isCancel = true;
                    myPlayer.hasPull = false;

                    myFSM.SetState(StateKind.Attack_heavy);
                    OnExit();

                    attackNum = 0;
                    myPlayer.attackTimer = 0;

                    myFSM.CurrentState.OnEnter();
                    break;
            }
        }

        public override void OnExit()
        {
            Debug.Log("Attack_normal Exit");

            myPlayer.attackTimer = myPlayer.attackTime;
            myPlayer.Set_normal_1(0);
            myPlayer.Set_normal_2(0);
            myPlayer.Set_normal_3(0);
            myPlayer.Set_canCancel(1);

            if (!isCancel)
            {
                if (myPlayer.hasPull && myPlayer.isGrounded)
                {
                    attackNum = 0;
                    myPlayer.attackTimer = 0;

                    myFSM.SetState(StateKind.Attack_heavy);
                }
                else if (preTimer > 0 && myPlayer.isGrounded)
                {
                    myFSM.SetState(StateKind.Attack_normal);
                }
                else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)
                {
                    myPlayer.hasFall = true;
                    myFSM.SetState(StateKind.Move);
                }
                else if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)
                {
                    myFSM.SetState(StateKind.Idle);
                }
                else
                {
                    myFSM.SetState(StateKind.Fall);
                }

                myFSM.CurrentState.OnEnter();
            }
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {
            //Ԥ����
            if(preTimer > 0)
            {
                preTimer -= Time.deltaTime;
            }
            if(Input.GetKeyDown(KeyCode.J))
            {
                preTimer = preTime;  
            }
            //�ػ���ȡ
            if(Input.GetKeyDown(KeyCode.J) || Input.GetKeyUp(KeyCode.J))
            {
                myPlayer.hasPull = false;
            }

            if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//����
            {
                isCancel = true;
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.I) && myPlayer.isGrounded)//���ܹ���
            {

            }
        }
    }

    public class AttackState_sky : BaseState
    {
        public Rigidbody2D myRigidBody;
        //�����˺�
        public int attackDamage;
        //�Ƿ�ȡ��
        public bool isCancel = false;

        public AttackState_sky(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
            attackDamage = myPlayer.attackDamage_normal;
        }
        public override void OnEnter()
        {
            Debug.Log("Attack_sky Enter");
        }

        public override void OnExit()
        {
            Debug.Log("Attack_sky Exit");

            if (!isCancel)
            {
                if (myPlayer.isGrounded)
                {
                    myFSM.SetState(StateKind.Idle);
                }
                else
                {
                    myFSM.SetState(StateKind.Fall);
                }

                myFSM.CurrentState.OnEnter();
            }
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {
            {
                if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//����
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
        public Rigidbody2D myRigidBody;
        //�����˺�
        public int attackDamage;
        //�����ƶ�����
        public float attackMoveForce;
        //�Ƿ�ȡ��
        public bool isCancel = false;

        public AttackState_heavy(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
            attackDamage = myPlayer.attackDamage_normal;
            attackMoveForce = myPlayer.attackMoveForce_heavy;
        }
        public override void OnEnter()
        {
            Debug.Log("Attack_heavy Enter");
            isCancel = false;
            //ʹ�����ͣ��(Maybe)
            myRigidBody.velocity = Vector2.zero;

            if (myPlayer.hasPull)
            {
                myPlayer.animator.Play("Attack_heavy_ready_player");
                myPlayer.Set_canCancel(0);
            }
            else
            {
                myPlayer.animator.Play("Attack_heavy_player");
                myPlayer.Set_canCancel(1);
            }

            myPlayer.hasPull = false;
        }

        public override void OnExit()
        {
            Debug.Log("Attack_heavy Exit");

            myPlayer.hasPull = false;
            myPlayer.Set_heavy(0);
            myPlayer.Set_canCancel(1);

            if (!isCancel)
            {
                if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)
                {
                    myFSM.SetState(StateKind.Idle);
                }
                else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)
                {
                    myPlayer.hasFall = true;
                    myFSM.SetState(StateKind.Move);
                }
                else
                {
                    myFSM.SetState(StateKind.Fall);
                }

                myFSM.CurrentState.OnEnter();
            }
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//����
            {
                isCancel = true;
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.I) && myPlayer.isGrounded)//���ܹ���
            {

            }
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

        public DashState(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myObject.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            //����ͨ������̶�����ٶȺ�ȡ������ʵ��
            myRigidBody.gravityScale = 0;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || !myPlayer.isGrounded)
            {            
                myRigidBody.velocity = new Vector2(myPlayer.dashSpeed * myPlayer.faceDir, 0);

                if(myPlayer.isGrounded)
                {
                    myPlayer.animator.Play("Dash_player");
                }
                else
                {
                    myPlayer.animator.Play("Dash_sky_player");
                }
            }
            else
            {      
                myRigidBody.velocity = new Vector2(-myPlayer.dashSpeed * myPlayer.faceDir, 0);
                myPlayer.animator.Play("Dash_back_player");
            }
        }

        public override void OnExit()
        {
            Debug.Log("Dash Exit");
            myRigidBody.gravityScale = 1;
            myRigidBody.velocity = new Vector2(0, 0);
            myPlayer.dashColdTimer = myPlayer.dashCold;

            if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)
            {
                myFSM.SetState(StateKind.Idle);
            }
            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)
            {
                myPlayer.hasFall = true;
                myFSM.SetState(StateKind.Move);
            }
            else
            {
                myFSM.SetState(StateKind.Fall);
            }
            myFSM.CurrentState.OnEnter();
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnUpdate()
        {

        }
    }

    public class HurtState : BaseState
    {
        public HurtState(StateMachine fsm) : base(fsm)
        {

        }
        public override void OnEnter()
        {
            myPlayer.animator.Play("Hurt_player");
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
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnUpdate()
        {
            
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
