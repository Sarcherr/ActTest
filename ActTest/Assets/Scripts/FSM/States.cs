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
            Debug.Log("Idle Enter");

            //提升线性阻尼防止角色停下时滑出太远
            myRigidBody.drag = 1;

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
            Debug.Log("Idle Exit");

            //myPlayer.animator.SetBool("isIdle", false);
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

            if (Input.GetKeyDown(KeyCode.LeftShift) && myPlayer.dashColdTimer <= 0)//闪避
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (false)//技能攻击
            {

            }
            else if (false)//重攻击
            {

            }
            else if (Input.GetMouseButtonDown(0))//轻攻击
            {
                myFSM.SetState(StateKind.Attack_normal);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.Space))//跳跃
            {
                myFSM.SetState(StateKind.Jump);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if(!myPlayer.isGrounded)//下坠
            {
                myFSM.SetState(StateKind.Fall);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && myPlayer.isGrounded)//移动
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
            Debug.Log("Move Enter");
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
            Debug.Log("Move Exit");
        }

        public override void OnFixedUpdate()
        {
            //调用水平移动方法
            myPlayer.MoveHorizontal();

            if (myPlayer.canCancel)
            {
                myPlayer.animator.Play("Move_player");  
                myRigidBody.gravityScale = 1;
            }
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && myPlayer.dashColdTimer <= 0)//闪避
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (false)//技能攻击
            {

            }
            else if (false)//重攻击
            {

            }
            else if (Input.GetMouseButtonDown(0))//轻攻击
            {
                myFSM.SetState(StateKind.Attack_normal);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && myPlayer.isGrounded)//跳跃
            {
                myFSM.SetState(StateKind.Jump);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if(!myPlayer.isGrounded)
            {
                myFSM.SetState(StateKind.Fall);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }    
            else if(!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)//松开AD且未在坠落
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
            Debug.Log("Fall Enter");
            myPlayer.animator.Play("Fall_player");
            //下坠时增大重力
            myRigidBody.gravityScale = myPlayer.fallGravity;
        }

        public override void OnExit()
        {
            Debug.Log("Fall Exit");
            myRigidBody.gravityScale = 1;
        }

        public override void OnFixedUpdate()
        {
            //调用水平移动方法
            myPlayer.MoveHorizontal();
            //限制最大下坠速度
            speedY = Mathf.Min(myRigidBody.velocity.y, myPlayer.maxFallSpeed);
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, speedY);
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && myPlayer.dashColdTimer <= 0)//闪避
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (false)//技能攻击
            {

            }
            else if (false)//重攻击
            {

            }
            else if (Input.GetMouseButtonDown(0))//轻攻击
            {
                myFSM.SetState(StateKind.Attack_normal);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)//移动
            {
                myPlayer.hasFall = true;
                myFSM.SetState(StateKind.Move);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)//松开AD且未在坠落
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
            Debug.Log("Jump Enter");

            //获取垂直方向速度
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, myPlayer.jumpForce);
            //暂时禁用groundsensor避免跳跃初期isgrounded仍为true
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
            //调用水平移动方法
            myPlayer.MoveHorizontal();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && myPlayer.dashColdTimer <= 0)//闪避
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (false)//技能攻击
            {

            }
            else if (false)//重攻击
            {

            }
            else if (Input.GetMouseButtonDown(0))//轻攻击
            {
                myFSM.SetState(StateKind.Attack_normal);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (myRigidBody.velocity.y <= 0)//下坠时切回下坠状态
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
        //攻击伤害
        public int attackDamage;
        //攻击移动幅度
        public float attackMoveForce;
        //攻击连段数
        public int attackNum = 0;
        //是否取消
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

            //使玩家先停下(Maybe)
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
            //取消前后摇
            //if(myPlayer.canCancel)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && myPlayer.dashColdTimer <= 0)//闪避
                {
                    isCancel = true;
                    myFSM.SetState(StateKind.Dash);
                    OnExit();
                    myFSM.CurrentState.OnEnter();
                }
                else if (false)//技能攻击
                {

                }
                else if (false)//重攻击
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
            //闪避通过给予固定冲刺速度和取消重力实现
            myRigidBody.gravityScale = 0;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || !myPlayer.isGrounded)
            {
                Debug.Log("Dash_ahead Enter");
                //timer = 0;               
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
                myFSM.SetState(StateKind.Fall);
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
            //    //极限闪避窗口期判定
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
