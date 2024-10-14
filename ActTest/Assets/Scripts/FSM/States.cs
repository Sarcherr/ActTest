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
            //提升线性阻尼防止角色停下时滑出太远
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
            //播放动画

            if(Input.GetKeyDown(KeyCode.LeftShift) && myPlayer.dashColdTimer <= 0)//闪避
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if(false)//技能攻击
            {
                
            }
            else if(false)//重攻击
            {

            }
            else if(Input.GetMouseButtonDown(1))//轻攻击
            {
                myFSM.SetState(StateKind.Attack_normal);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if(Input.GetKeyDown(KeyCode.Space))//跳跃
            {
                myFSM.SetState(StateKind.Jump);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }   
            else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || !myPlayer.isGrounded)//移动
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
            //保险,防止落地时OnUpdate未恢复重力
            myRigidBody.gravityScale = 1;
            Debug.Log("Move Exit");

            myPlayer.animator.SetBool("isFall", false);
            myPlayer.animator.SetBool("isMove", false);
        }

        public override void OnFixedUpdate()
        {
            //调用水平移动方法
            myPlayer.MoveHorizontal();

            //下坠同样整合在Move状态中
            if (!myPlayer.isGrounded && myRigidBody.velocity.y <= 0)
            {
                myPlayer.animator.SetBool("isFall", true);
                myPlayer.animator.SetBool("isMove", false);
                //下坠时增大重力
                myRigidBody.gravityScale = myPlayer.fallGravity;
                //限制最大下坠速度
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
            else if (false)//轻攻击
            {

            }
            else if (Input.GetKeyDown(KeyCode.Space) && myPlayer.isGrounded)//跳跃
            {
                myFSM.SetState(StateKind.Jump);
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
            myPlayer.animator.SetBool("isJump", true);
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
            else if (false)//轻攻击
            {

            }
            else if (myRigidBody.velocity.y <= 0)//下坠时切回整合了下坠的Move状态
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
        //攻击伤害
        public int attackDamage;
        //攻击移动幅度
        public float attackMoveForce = 1.5f;
        //攻击连段数
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
            //闪避通过给予固定冲刺速度和取消重力实现
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
