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
            Debug.Log(myPlayer.isGrounded);
            Debug.Log("Idle Enter");
        }

        public override void OnExit()
        {
            Debug.Log(myPlayer.isGrounded);
            Debug.Log("Idle Exit");
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
            else if(false)//轻攻击
            {

            }
            else if(Input.GetKeyDown(KeyCode.Space))//跳跃
            {
                myFSM.SetState(StateKind.Jump);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }   
            else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))//移动
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
        }

        public override void OnExit()
        {
            //保险,防止落地时OnUpdate未恢复重力
            myRigidBody.gravityScale = 1;
            Debug.Log("Move Exit");
        }

        public override void OnFixedUpdate()
        {
            //调用水平移动方法
            myPlayer.MoveHorizontal();
            //下坠同样整合在Move状态中
            if (!myPlayer.isGrounded && myRigidBody.velocity.y < 0)
            {
                myPlayer.animator.SetBool("isFall", true);
                //下坠时增大重力
                myRigidBody.gravityScale = myPlayer.fallGravity;
                //限制最大下坠速度
                speedY = Mathf.Min(myRigidBody.velocity.y, myPlayer.maxFallSpeed);
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, speedY);
            }
            else
            {
                myPlayer.animator.SetBool("isFall", false);
                myRigidBody.gravityScale = 1;
            }
        }

        public override void OnUpdate()
        {
            //播放动画

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
        private Rigidbody2D myRigidBody;
        private float timer;

        public DashState(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myObject.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            //闪避通过给予固定冲刺速度和取消重力实现
            Debug.Log("Dash Enter");
            timer = 0;
            myPlayer.inDashWindow = true;
            myRigidBody.gravityScale = 0;
            myRigidBody.velocity = new Vector2 (myPlayer.dashSpeed * myPlayer.faceDir, 0);

            myPlayer.dashColdTimer = myPlayer.dashCold;
        }

        public override void OnExit()
        {
            Debug.Log("Dash Exit");
            myObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            myRigidBody.velocity = new Vector2(0, 0);
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
                //极限闪避窗口期判定
                myPlayer.inDashWindow = false;
            }
            else if(timer > myPlayer.dashTime)
            {
                //TODO:添加检测，闪避结束后在空中切坠落，在地面切待机
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
