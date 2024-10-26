using System.Collections;
using System.Collections.Generic;
using Unit;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace FSM
{
    public class IdleState : BaseState
    {
        public Rigidbody2D myRigidBody;
        //切换长待机动画时间
        private float idleTime = 9;
        //切换长待机动画计时器
        private float idleTimer = 0;
        private bool isIdle = false;

        public IdleState(StateMachine fsm):base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            idleTimer = idleTime;
            isIdle = false;
            //提升线性阻尼防止角色停下时滑出太远
            myRigidBody.drag = myPlayer.stopDrag;

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
            idleTimer = idleTime;
            isIdle = false;
            myRigidBody.drag = 0;
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {
            if(myPlayer.canCancel && !isIdle)
            {
                myPlayer.animator.Play("Idle_player");
            }

            if(idleTimer > 0)
            {
                idleTimer -= Time.deltaTime;
            }
            else
            {
                idleTimer = idleTime;
                isIdle = true;
                myPlayer.animator.Play("Idle_long_player");
            }

            if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//闪避
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (!myPlayer.isGrounded)//下坠
            {
                myFSM.SetState(StateKind.Fall);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.I) && myPlayer.isGrounded)//技能攻击
            {
                if (myPlayer.UseSkill())
                {
                    OnExit();
                }
            }
            else if (Input.GetKeyDown(KeyCode.J) && myPlayer.isGrounded)//轻攻击
            {
                myFSM.SetState(StateKind.Attack_normal);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.K) && myPlayer.isGrounded)//跳跃
            {
                myFSM.SetState(StateKind.Jump);
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
            if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//闪避
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (!myPlayer.isGrounded)//下坠
            {
                myFSM.SetState(StateKind.Fall);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.I) && myPlayer.isGrounded)//技能攻击
            {
                if (myPlayer.UseSkill())
                {
                    OnExit();
                }
            }
            else if (Input.GetKeyDown(KeyCode.J) && myPlayer.isGrounded)//轻攻击
            {
                myFSM.SetState(StateKind.Attack_normal);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.K) && myPlayer.isGrounded)//跳跃
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
            //下坠时增大重力
            myRigidBody.gravityScale = myPlayer.fallGravity;
        }

        public override void OnExit()
        {
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
            if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//闪避
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.J) && myPlayer.canAttack_sky)//空中攻击
            {
                myFSM.SetState(StateKind.Attack_sky);
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
            myRigidBody.gravityScale = myPlayer.jumpGravity;
            //获取垂直方向速度
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, myPlayer.jumpForce);
            //暂时禁用groundsensor避免跳跃初期isgrounded仍为true
            myPlayer.groundSensor.Disable(0.2f);
            myPlayer.animator.Play("Jump_player");
        }

        public override void OnExit()
        {
            myRigidBody.gravityScale = 1;
        }

        public override void OnFixedUpdate()
        {
            //调用水平移动方法
            myPlayer.MoveHorizontal();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//闪避
            {
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.J) && myPlayer.canAttack_sky)//空中攻击
            {
                myFSM.SetState(StateKind.Attack_sky);
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
        //攻击移动幅度
        public float attackMoveForce;
        //攻击连段数
        public int attackNum = 0;
        //是否取消
        public bool isCancel = false;

        //预输入窗口
        public float preTime = 0.2f;
        //预输入计时器
        public float preTimer = 0;

        public AttackState_normal(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
            attackMoveForce = myPlayer.attackMoveForce_normal;
        }
        public override void OnEnter()
        {
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

            //使玩家先停下(Maybe)
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
            myRigidBody.velocity = new Vector2(0, 0);
            myPlayer.attackTimer = myPlayer.attackTime;
            myPlayer.Set_normal_1(0);
            myPlayer.Set_normal_2(0);
            myPlayer.Set_normal_3(0);
            myPlayer.Set_canCancel(1);

            if (!isCancel)
            {
                if (myPlayer.hasPull && myPlayer.isGrounded)
                {
                    myRigidBody.velocity = Vector2.zero;
                    attackNum = 0;
                    myPlayer.attackTimer = 0;

                    myFSM.SetState(StateKind.Attack_heavy);
                }
                else if (preTimer > 0 && myPlayer.isGrounded)
                {
                    myRigidBody.velocity = Vector2.zero;
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
            //预输入
            if(preTimer > 0)
            {
                preTimer -= Time.deltaTime;
            }
            if(Input.GetKeyDown(KeyCode.J))
            {
                preTimer = preTime;  
            }
            //重击读取
            if(Input.GetKeyDown(KeyCode.J) || Input.GetKeyUp(KeyCode.J))
            {
                myPlayer.hasPull = false;
            }

            if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//闪避
            {
                isCancel = true;
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.I) && myPlayer.isGrounded && myPlayer.canCancel)//技能攻击
            {
                if (myPlayer.UseSkill())
                {
                    isCancel = true;
                    OnExit();
                }
            }
        }
    }

    public class AttackState_sky : BaseState
    {
        public Rigidbody2D myRigidBody;
        private float speedY;
        //是否取消
        public bool isCancel = false;

        public AttackState_sky(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            isCancel = false;
            myPlayer.canAttack_sky = false;
            //下坠时增大重力
            myRigidBody.gravityScale = myPlayer.fallGravity;

            myPlayer.animator.Play("Attack_sky_player");
        }

        public override void OnExit()
        {
            myRigidBody.gravityScale = 1;

            if (!isCancel)
            {
                if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)
                {
                    myPlayer.hasFall = true;
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
            //限制最大下坠速度
            speedY = Mathf.Min(myRigidBody.velocity.y, myPlayer.maxFallSpeed);
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, speedY);
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//闪避
            {
                isCancel = true;
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
        }
    }

    public class AttackState_heavy : BaseState
    {
        public Rigidbody2D myRigidBody;
        //是否取消
        public bool isCancel = false;

        public AttackState_heavy(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            isCancel = false;
            //使玩家先停下(Maybe)
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
            myRigidBody.velocity = new Vector2(0, 0);
            myPlayer.hasPull = false;
            myPlayer.Set_heavy(0);
            myPlayer.Set_canCancel(1);
            myPlayer.Set_isInvincible(0);
            myPlayer.Set_isUnstoppable(0);

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
            if (Input.GetKeyDown(KeyCode.L) && myPlayer.dashColdTimer <= 0)//闪避
            {
                isCancel = true;
                myFSM.SetState(StateKind.Dash);
                OnExit();
                myFSM.CurrentState.OnEnter();
            }
            else if (Input.GetKeyDown(KeyCode.I) && myPlayer.isGrounded && myPlayer.canCancel)//技能攻击
            {
                if (myPlayer.UseSkill())
                {
                    isCancel = true;
                    OnExit();
                }
            }
        }
    }

    public class AttackState_skill_1 : BaseState
    {
        public Rigidbody2D myRigidBody;

        public AttackState_skill_1(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            myPlayer.Set_isUnstoppable(1);
            //使玩家先停下(Maybe)
            myRigidBody.velocity = Vector2.zero;
            myPlayer.animator.Play("Attack_skill_1_player");

        }

        public override void OnExit()
        {
            myPlayer.Set_skill_1_1(0);
            myPlayer.Set_isInvincible(0);
            myPlayer.Set_isUnstoppable(0);

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

    public class AttackState_skill_2 : BaseState
    {
        public Rigidbody2D myRigidBody;

        public AttackState_skill_2(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myPlayer.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            myPlayer.Set_isUnstoppable(1);
            myPlayer.Set_end(0);
            //使玩家先停下(Maybe)
            myRigidBody.velocity = Vector2.zero;

            switch(myPlayer.skillNow)
            {
                case SkillHasUse.skill_2_1:
                    myPlayer.animator.Play("Attack_skill_2_1_player");
                    break;

                case SkillHasUse.skill_2_2:
                    myPlayer.animator.Play("Attack_skill_2_2_player");
                    break;

                case SkillHasUse.skill_2_3:

                    myPlayer.animator.Play("Attack_skill_2_3_player");
                    break;
            }
        }

        public override void OnExit()
        {
            myPlayer.Set_skill_2_1(0);
            myPlayer.Set_skill_2_2(0);
            myPlayer.Set_skill_2_3(0);
            myPlayer.Set_end(0);
            myPlayer.Set_isInvincible(0);
            myPlayer.Set_isUnstoppable(0);
            

            if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)
            {
                myFSM.SetState(StateKind.Idle);
            }
            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)
            {
                myFSM.SetState(StateKind.Move);
            }
            else
            {
                myFSM.SetState(StateKind.Fall);
            }

            myFSM.CurrentState.OnEnter();
            if(myPlayer.skillNow == SkillHasUse.skill_2_3)
            {
                myPlayer.TP_end();
                myPlayer.Focus_Player();
                myPlayer.CameraFocus(0);
            }
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
            myObject.layer = LayerMask.NameToLayer("Player_dash");
            //闪避通过给予固定冲刺速度和取消重力实现
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
            if (!myPlayer.inDashWindow)
            {
                myObject.layer = LayerMask.NameToLayer("Player");
                myPlayer.Set_isInvincible(0);
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

            myPlayer.Set_inDashWindow(0);
            myPlayer.Set_inDashWindow_back(0);
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnUpdate()
        {

        }
    }

    public class DashState_extreme : BaseState
    {
        private Rigidbody2D myRigidBody;

        public DashState_extreme(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myObject.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            myObject.layer = LayerMask.NameToLayer("Player_dash");
            //闪避通过给予固定冲刺速度和取消重力实现
            myRigidBody.gravityScale = 0;
            myRigidBody.velocity = new Vector2(myPlayer.dashSpeed * myPlayer.faceDir * 1.5f, 0);
            myPlayer.Set_isInvincible(1);
            myPlayer.animator.Play("Dash_extreme_player");
        }

        public override void OnExit()
        {
            myObject.layer = LayerMask.NameToLayer("Player");
            myPlayer.Set_inDashWindow(0);
            myPlayer.Set_inDashWindow_back(0);
            myPlayer.Set_isInvincible(0);
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
        private Rigidbody2D myRigidBody;
        public HurtState(StateMachine fsm) : base(fsm)
        {
            myRigidBody = myObject.GetComponent<Rigidbody2D>();
        }
        public override void OnEnter()
        {
            myPlayer.Set_isInvincible(1);
            if(myPlayer.isGrounded)
            {
                myRigidBody.drag = myPlayer.stopDrag;
            }
            myRigidBody.gravityScale = myPlayer.fallGravity;
            myPlayer.animator.Play("Hurt_player");
        }

        public override void OnExit()
        {
            myRigidBody.drag = 0;
            myPlayer.Set_isInvincible(0);

            if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)
            {
                myRigidBody.gravityScale = 1;
                myFSM.SetState(StateKind.Idle);
            }
            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && myPlayer.isGrounded)
            {
                myPlayer.hasFall = true;
                myRigidBody.gravityScale = 1;
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

    public class DeadState : BaseState
    {
        public DeadState(StateMachine fsm) : base(fsm)
        {

        }
        public override void OnEnter()
        {
            myPlayer.Set_isInvincible(1);
            myObject.GetComponent<Rigidbody2D>().drag = myPlayer.stopDrag;
            myPlayer.animator.Play("Dead_player");
        }

        public override void OnExit()
        {
            //游戏失败
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