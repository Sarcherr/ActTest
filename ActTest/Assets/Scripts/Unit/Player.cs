using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;
using FSM;
using Cinemachine;
using System.Xml.Linq;

namespace Unit
{
    //继承的数据成员:
    //(最大生命值)int maxHP;
    //(移动速度)  float moveSpeed;
    //(跳跃力度)  float jumpForce
    //(朝向)      int faceDir;
    //(当前生命值)int currentHP;
    //(是否触地)  bool isGrounded;
    //(触地传感器)GroundSensor groundSensor;
    /// <summary>
    /// 玩家角色类
    /// </summary>
    public class Player : Unit
    {
        /// <summary>
        /// 上升重力
        /// </summary>
        [Header("上升重力")] public float jumpGravity;
        /// <summary>
        /// 下坠重力
        /// </summary>
        [Header("下坠重力")] public float fallGravity;
        /// <summary>
        /// 最大下坠速度
        /// </summary>
        [Header("最大下坠速度")] public float maxFallSpeed;
        /// <summary>
        /// 移动停止阻力  
        /// </summary>
        [Header("移动停止阻力")] public float stopDrag;
        /// <summary>
        /// 相机回正时间(次元斩) 
        /// </summary>
        [Header("相机回正速度(次元斩) ")] public float ResetTime;


        /// <summary>
        /// 闪避速度
        /// </summary>
        [Header("闪避速度")] public float dashSpeed;
        /// <summary>
        /// 闪避冷却    
        /// </summary>
        [Header("闪避冷却")] public float dashCold;
        /// <summary>
        /// 前闪获得架势值
        /// </summary>
        [Header("前闪获得架势值")] public int dashGain_SP;
        /// <summary>
        /// 后闪获得血瓶条
        /// </summary>
        [Header("后闪获得血瓶条")] public int dashGain_Cure;

        /// <summary>
        /// 霸体减伤幅度(0~1)
        /// </summary>
        [Header("霸体减伤幅度(0~1)")] public float damageScale;

        /// <summary>
        /// 最大架势值
        /// </summary>
        [Header("最大架势值")] public int maxSP;
        /// <summary>
        /// 最大血瓶条
        /// </summary>
        [Header("最大血瓶条")] public int maxCure;

        /// <summary>
        /// 技能选择(true登龙/false次元斩)
        /// </summary>
        [Header("技能选择(true登龙/false次元斩)")] public bool mySkill;
        /// <summary>
        /// 攻击连段窗口期
        /// </summary>
        [Header("攻击连段窗口期")] public float attackTime;

        /// <summary>
        /// 血瓶治疗量
        /// </summary>
        [Header("血瓶治疗量")] public int cureQuantity;
        /// <summary>
        /// 血瓶消耗
        /// </summary>
        [Header("血瓶消耗")] public int cureConsumption;
        /// <summary>
        /// 登龙SP消耗
        /// </summary>
        [Header("登龙SP消耗")] public int attackConsumption_skill_1;
        /// <summary>
        /// 次元斩1阶SP消耗
        /// </summary>
        [Header("次元斩1阶SP消耗")] public int attackConsumption_skill_2_1;
        /// <summary>
        /// 次元斩2阶SP消耗
        /// </summary>
        [Header("次元斩2阶SP消耗")] public int attackConsumption_skill_2_2;
        /// <summary>
        /// 次元斩3阶SP消耗
        /// </summary>
        [Header("次元斩3阶SP消耗")] public int attackConsumption_skill_2_3;

        /// <summary>
        /// 普通(轻)攻击移动幅度
        /// </summary>
        [Header("普通(轻)攻击移动幅度")] public float attackMoveForce_normal;
        /// <summary>
        /// 重攻击移动幅度
        /// </summary>
        [Header("重攻击移动幅度")] public float attackMoveForce_heavy;
        /// <summary>
        /// 登龙1段移动幅度
        /// </summary>
        [Header("登龙1段移动幅度")] public float attackMoveForce_skill_1;

        /// <summary>
        /// 普通(轻)攻击伤害
        /// </summary>
        [Header("普通(轻)攻击伤害")] public int attackDamage_normal;
        /// <summary>
        /// 重攻击伤害
        /// </summary>
        [Header("重攻击伤害")] public int attackDamage_heavy;
        /// <summary>
        /// 空中攻击伤害
        /// </summary>
        [Header("空中攻击伤害")] public int attackDamage_sky;
        /// <summary>
        /// 技能攻击伤害_1(登龙)
        /// </summary>
        [Header("技能攻击伤害_1_1(登龙1段)")] public int attackDamage_skill_1;
        /// <summary>
        /// 技能攻击伤害_2_1(次元斩1层)
        /// </summary>
        [Header("技能攻击伤害_2_1(次元斩1层)")] public int attackDamage_skill_2_1;
        /// <summary>
        /// 技能攻击伤害_2_2(次元斩2层)
        /// </summary>
        [Header("技能攻击伤害_2_2(次元斩2层)")] public int attackDamage_skill_2_2;
        /// <summary>
        /// 技能攻击伤害_2_3(次元斩3层)
        /// </summary>
        [Header("技能攻击伤害_2_2(次元斩3层)")] public int attackDamage_skill_2_3;
        /// <summary>
        /// 普通(轻)攻击命中回复SP
        /// </summary>
        [Header("普通(轻)攻击命中回复SP")] public int attackGain_normal;
        /// <summary>
        /// 重攻击命中回复SP
        /// </summary>
        [Header("重攻击命中回复SP")] public int attackGain_heavy;
        /// <summary>
        /// 空中攻击命中回复SP
        /// </summary>
        [Header("空中攻击命中回复SP")] public int attackGain_sky;


        /// <summary>
        /// 当前架势值
        /// </summary>
        [HideInInspector] public int currentSP = 0;
        /// <summary>
        /// 当前血瓶条
        /// </summary>
        [HideInInspector] public int currentCure = 0;
        /// <summary>
        /// 攻击连段计时
        /// </summary>
        [HideInInspector] public float attackTimer = 0;
        /// <summary>
        /// 闪避冷却计时
        /// </summary>
        [HideInInspector] public float dashColdTimer = 0;
        /// <summary>
        /// 当前释放的技能
        /// </summary>
        [HideInInspector] public SkillHasUse skillNow = SkillHasUse.skill_default;
        /// <summary>
        /// 是否处于极限闪避窗口期(前)
        /// </summary>
        [HideInInspector] public bool inDashWindow = false;
        /// <summary>
        /// 是否处于极限闪避窗口期(后)
        /// </summary>
        [HideInInspector] public bool inDashWindow_back = false;
        /// <summary>
        /// 是否落地
        /// </summary>
        [HideInInspector] public bool hasFall = false;
        /// <summary>
        /// 是否长按重击
        /// </summary>
        [HideInInspector] public bool hasPull = false;
        /// <summary>
        /// 当前状态可否取消(只用于闪避攻击等)
        /// </summary>
        [HideInInspector] public bool canCancel = true;
        /// <summary>
        /// 当前可否空中攻击
        /// </summary>
        [HideInInspector] public bool canAttack_sky = true;
        /// <summary>
        /// 是否处于霸体状态
        /// </summary>
        [HideInInspector] public bool isUnstoppable = false;
        /// <summary>
        /// 是否处于无敌状态
        /// </summary>
        [HideInInspector] public bool isInvincible = false;

        /// <summary>
        /// 是否产生落点偏差
        /// </summary>
        [HideInInspector] public bool hasBias = false;
        /// <summary>
        /// 落点偏差坐标
        /// </summary>
        [HideInInspector] public Vector2 biasPos;

        /// <summary>
        /// 攻击判定框normal_1
        /// </summary>
        [HideInInspector] public GameObject attackRange_normal_1;
        /// <summary>
        /// 攻击判定框normal_2
        /// </summary>
        [HideInInspector] public GameObject attackRange_normal_2;
        /// <summary>
        /// 攻击判定框normal_3
        /// </summary>
        [HideInInspector] public GameObject attackRange_normal_3;
        /// <summary>
        /// 攻击判定框sky
        /// </summary>
        [HideInInspector] public GameObject attackRange_sky;
        /// <summary>
        /// 攻击判定框heavy
        /// </summary>
        [HideInInspector] public GameObject attackRange_heavy;
        /// <summary>
        /// 攻击判定框skill_1_1
        /// </summary>
        [HideInInspector] public GameObject attackRange_skill_1_1;
        /// <summary>
        /// 攻击判定框skill_1_2
        /// </summary>
        [HideInInspector] public GameObject attackRange_skill_1_2;
        /// <summary>
        /// 攻击判定框skill_2_1
        /// </summary>
        [HideInInspector] public GameObject attackRange_skill_2_1;
        /// <summary>
        /// 攻击判定框skill_2_2
        /// </summary>
        [HideInInspector] public GameObject attackRange_skill_2_2;
        /// <summary>
        /// 攻击判定框skill_2_3
        /// </summary>
        [HideInInspector] public GameObject attackRange_skill_2_3;

        /// <summary>
        /// 次元斩基准点middle
        /// </summary>
        [HideInInspector] public GameObject skill_2_3_middle;
        /// <summary>
        /// 次元斩基准点end
        /// </summary>
        [HideInInspector] public GameObject skill_2_3_end;
        /// <summary>
        /// 受击特效基准点
        /// </summary>
        [HideInInspector] public GameObject hurter;
        /// <summary>
        /// 恢复特效基准点
        /// </summary>
        [HideInInspector] public GameObject curer;

        /// <summary>
        /// 角色动画机
        /// </summary>
        [HideInInspector] public Animator animator;
        /// <summary>
        /// 角色刚体
        /// </summary>
        [HideInInspector] public Rigidbody2D myRigidBody;
        /// <summary>
        /// 角色碰撞体
        /// </summary>
        [HideInInspector] public Collider2D myCollider;
        /// <summary>
        /// 角色摄像头
        /// </summary>
        [HideInInspector] public CinemachineVirtualCamera myCamera;
        /// <summary>
        /// 角色状态机
        /// </summary>
        [HideInInspector] public StateMachine fsm;
        /// <summary>
        /// 游戏管理器
        /// </summary>
        [HideInInspector] public GameManager gameManager;

        void Start()
        {
            //数据成员初始化
            faceDir = 1;
            currentHP = maxHP;

            groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();
            myRigidBody = GetComponent<Rigidbody2D>();
            myCollider = GetComponent<Collider2D>();

            fsm = new StateMachine(gameObject);
            fsm.OnEnable();
            
            animator = GetComponent<Animator>();
            myCamera = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
            gameManager = GameObject.Find("Canvas").GetComponent<GameManager>();

            attackRange_normal_1 = transform.Find("AttackRange_normal_1").gameObject;
            attackRange_normal_2 = transform.Find("AttackRange_normal_2").gameObject;
            attackRange_normal_3 = transform.Find("AttackRange_normal_3").gameObject;
            attackRange_heavy = transform.Find("AttackRange_heavy").gameObject;
            attackRange_sky = transform.Find("AttackRange_sky").gameObject;
            attackRange_skill_1_1 = transform.Find("AttackRange_skill_1_1").gameObject;
            attackRange_skill_1_2 = transform.Find("AttackRange_skill_1_2").gameObject;
            attackRange_skill_2_1 = transform.Find("AttackRange_skill_2_1").gameObject;
            attackRange_skill_2_2 = transform.Find("AttackRange_skill_2_2").gameObject;
            attackRange_skill_2_3 = transform.Find("AttackRange_skill_2_3").gameObject;

            skill_2_3_middle = transform.Find("skill_2_3_middle").gameObject;
            skill_2_3_end = transform.Find("skill_2_3_end").gameObject;
            hurter = transform.Find("Hurter").gameObject;
            curer = transform.Find("Curer").gameObject;
        }


        void Update()
        {
            fsm.OnUpdate();

            GetGroundState();
            AttackTimeCount();
            DashColdDown();
            Heal();

            if(Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log(fsm.CurrentStateKind);
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Hurt(10);
            }
            if(Input.GetKeyDown(KeyCode.E))
            {
                SPChange(20);
                CureChange(20);
            }
            if(Input.GetKeyDown(KeyCode.F))
            {
                mySkill = !mySkill;
            }
        }

        void FixedUpdate()
        {
            fsm.OnFixedUpdate();
        }

        public void GetGroundState()
        {
            if (!isGrounded && groundSensor.GetComponent<GroundSensor>().State())
            {
                isGrounded = true;
                canAttack_sky = true;
            }

            if (isGrounded && !groundSensor.GetComponent<GroundSensor>().State())
            {
                isGrounded = false;
            }
        }

        /// <summary>
        /// 水平移动输入
        /// </summary>
        public void MoveHorizontal()
        {
            //获取水平移动输入,修改朝向和速度
            float inputX = Input.GetAxis("Horizontal");

            if (inputX > 0)
            {
                faceDir = 1;
            }
            else if (inputX < 0)
            {
                faceDir = -1;
            }

            transform.localScale = new Vector3(faceDir, 1, 1);
            if(inputX != 0)
            {
                myRigidBody.velocity = new Vector2(faceDir * moveSpeed, myRigidBody.velocity.y);
            }
        }

        /// <summary>
        /// 攻击连段计时
        /// </summary>
        public void AttackTimeCount()
        {
            if(attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
        }

        /// <summary>
        /// 闪避冷却
        /// </summary>
        public void DashColdDown()
        {       
            if(dashColdTimer > 0)
            {
                dashColdTimer -= Time.deltaTime;
            }
        }

        /// <summary>
        /// 释放技能
        /// </summary>
        public bool UseSkill()
        {
            
            if(mySkill)
            {
                if(currentSP >= attackConsumption_skill_1)
                {
                    SPChange(-attackConsumption_skill_1);

                    fsm.SetState(StateKind.Attack_skill_1);
                    fsm.CurrentState.OnEnter();

                    return true;
                }
                else
                {
                    //ui效果提示等
                    gameManager.Shake(gameManager._SP);

                    return false;
                }
            }
            else
            {
                if (currentSP >= attackConsumption_skill_2_1)
                {
                    //判断阶数
                    if(currentSP >= attackConsumption_skill_2_3)
                    {
                        SPChange(-attackConsumption_skill_2_3);
                        skillNow = SkillHasUse.skill_2_3;

                        //DetectLayer_9:Edge(防止闪出地图外)
                        RaycastHit2D hit = 
                            Physics2D.Raycast(transform.position, new Vector2(faceDir, 0), 10.4f, 1 << LayerMask.NameToLayer("Edge"));
                        if(hit.collider != null)
                        {
                            hasBias = true;
                            float x_bias = hit.point.x - skill_2_3_end.transform.position.x;
                            biasPos = new Vector2(transform.position.x + x_bias, transform.position.y);
                        }
                        else
                        {
                            hasBias = false;
                            biasPos = Vector2.zero;
                        }
                    }
                    else if(currentSP >= attackConsumption_skill_2_2)
                    {
                        SPChange(-attackConsumption_skill_2_2);
                        skillNow = SkillHasUse.skill_2_2;
                    }
                    else
                    {
                        SPChange(-attackConsumption_skill_2_1);
                        skillNow = SkillHasUse.skill_2_1;
                    }

                    fsm.SetState(StateKind.Attack_skill_2);
                    fsm.CurrentState.OnEnter();

                    return true;
                }
                else
                {
                    //ui效果提示等
                    gameManager.Shake(gameManager._SP);

                    return false;
                }
            }
        }

        /// <summary>
        /// SP值变化(正负可能伴随UI相关效果调用)
        /// </summary>
        /// <param name="spChange"></param>
        public void SPChange(int spChange)
        {
            int _currentSP = currentSP + spChange;
            currentSP = Math.Clamp(_currentSP, 0, maxSP);

            //根据增减调用UI效果
            if(spChange > 0)
            {

            }
            else
            {

            }
        }
        /// <summary>
        /// 血瓶值变化(正负可能伴随UI相关效果调用)
        /// </summary>
        /// <param name="cureChange"></param>
        public void CureChange(int cureChange)
        {
            int _currentCure = currentCure + cureChange;
            currentCure = Math.Clamp(_currentCure, 0, maxCure);

            //根据增减调用UI效果
            if(cureChange > 0)
            {

            }
            else
            {

            }
        }

        /// <summary>
        /// 受伤
        /// </summary>
        /// <param name="damage"></param>
        public void Hurt(int damage)
        {
            //极限闪避
            if(inDashWindow || inDashWindow_back)
            {
                transform.gameObject.layer = LayerMask.NameToLayer("Player_dash");

                if (inDashWindow)
                {
                    UltimateEvasion(true);

                    Set_inDashWindow(0);
                    End_state_now();
                    fsm.SetState(StateKind.Dash_extreme);
                    fsm.CurrentState.OnEnter();
                }
                else
                {
                    UltimateEvasion(false);

                    Set_inDashWindow_back(0);
                    animator.speed = 0;
                    myRigidBody.velocity = Vector2.zero;
                    Invoke(nameof(KeepPlay), 0.1f);
                }
            }
            else if (isUnstoppable && !isInvincible)
            {
                gameManager.Shake(gameManager._HP);
                hurter.SetActive(true);
                Invoke(nameof(QuikDisable), 0.08f);
                currentHP -= (int)(damage * damageScale);
            }
            else if (!isInvincible)
            {
                gameManager.Shake(gameManager._HP);
                hurter.SetActive(true);
                currentHP -= damage;

                //受击
                End_state_now();
                fsm.SetState(StateKind.Hurt);
                fsm.CurrentState.OnEnter();
            }
            if (currentHP < 0)
            {
                hurter.SetActive(true);
                hurter.GetComponent<Animator>().speed = 0.7f;
                //死亡
                End_state_now();
                fsm.SetState(StateKind.Dead);
                fsm.CurrentState.OnEnter();
            }
        }

        /// <summary>
        /// 血瓶回血
        /// </summary>
        public void Heal()
        {
            if(Input.GetKeyDown(KeyCode.O))
            {
                if(currentCure >= cureConsumption)
                {
                    if(fsm.CurrentStateKind is not StateKind.Dead and not StateKind.Hurt
                        and not StateKind.Attack_normal and not StateKind.Attack_sky
                         and not StateKind.Attack_heavy and not StateKind.Attack_skill_1
                          and not StateKind.Attack_skill_2 and not StateKind.Dash_extreme)
                    {
                        curer.SetActive(false);
                        curer.SetActive(true);
                        int _currentHP = currentHP + cureQuantity;
                        _currentHP = Math.Clamp(_currentHP, 0, maxHP);
                        currentHP = _currentHP;

                        CureChange(-cureConsumption);
                    }
                }
                else
                {
                    //UI提示
                    gameManager.Shake(gameManager._Curer);
                }
            }
        }

        /// <summary>
        /// 动画恢复正常播放
        /// </summary>
        public void KeepPlay()
        {
            animator.speed = 1;
            myRigidBody.velocity = new Vector2(-dashSpeed * faceDir, 0);
        }

        /// <summary>
        /// 禁用受击特效(霸体受击用)
        /// </summary>
        public void QuikDisable()
        {
            hurter.SetActive(false);
        }

        /// <summary>
        /// 极限闪避获取资源
        /// </summary>
        public void UltimateEvasion(bool ahead)
        {
            if(ahead)
            {
                SPChange(dashGain_SP);
            }
            else
            {
                hurter.SetActive(true);
                Invoke(nameof(QuikDisable), 0.08f);
                CureChange(dashGain_Cure);
            }
        }

        /// <summary>
        /// 还原相机size
        /// </summary>
        /// <returns></returns>
        public IEnumerator ResetCamera()
        {
            float timer = 0;

            while(timer < ResetTime)
            {
                timer += Time.deltaTime;
                myCamera.m_Lens.OrthographicSize = Mathf.Lerp(3.95f, 7, timer/ResetTime);
                yield return null;
            }

            myCamera.m_Lens.OrthographicSize = 7;
        }

        #region 用于帧事件调用(其中int参数使用1/0表示true/false)
        /// <summary>
        /// 相机聚焦(1聚焦/0还原)
        /// </summary>
        /// <param name="isFocus"></param>
        public void CameraFocus(int isFocus)
        {
            if (isFocus == 1)
            {
                myCamera.m_Lens.OrthographicSize = 3.95f;
            }
            else
            {
                StartCoroutine(ResetCamera());
            }
        }
        public void Focus_Player()
        {
            myCamera.Follow = transform;
        }
        public void Focus_middle()
        {
            if(hasBias == false)
            {
                myCamera.Follow = skill_2_3_middle.transform;
            }
            else
            {
                myCamera.Follow = null;
                transform.position = biasPos;
                myCamera.Follow = skill_2_3_middle.transform;
            }
            hasBias = false;
            biasPos = Vector2.zero;
        }
        public void Focus_end()
        {
            myCamera.Follow = skill_2_3_end.transform;
        }
        /// <summary>
        /// 传送到end(次元斩)
        /// </summary>
        public void TP_end()
        {
            transform.position = skill_2_3_end.transform.position;
        }
        public void Set_end(int isTrue)
        {
            skill_2_3_end.SetActive(Convert.ToBoolean(isTrue));
        }
        /// <summary>
        /// 播放待机动画
        /// </summary>
        public void Play_idle()
        {
            animator.Play("Idle_player");
        }
        /// <summary>
        /// 播放移动动画
        /// </summary>
        public void Play_move()
        {
            animator.Play("Move_player");
        }
        /// <summary>
        /// 播放重攻击动画
        /// </summary>
        public void Play_attack_heavy()
        {
            animator.Play("Attack_heavy_player");
        }
        /// <summary>
        /// 施加位移推力(重攻击)
        /// </summary>
        public void AddImpulse()
        {
            myRigidBody.AddForce(new Vector2(faceDir * attackMoveForce_heavy, 0), ForceMode2D.Impulse);
        }
        /// <summary>
        /// 施加位移推力(登龙)
        /// </summary>
        public void AddImpulse_great()
        {
            myRigidBody.AddForce(new Vector2(faceDir * attackMoveForce_skill_1, 0), ForceMode2D.Impulse);
        }
        /// <summary>
        /// 速度归零
        /// </summary>
        public void ResetVelocity()
        {
            myRigidBody.velocity = Vector2.zero;
        }
        /// <summary>
        /// 终止当前状态
        /// </summary>
        public void End_state_now()
        {
            fsm.CurrentState.OnExit();
        }
        /// <summary>
        /// 设置是否为极限闪避窗口期
        /// </summary>
        /// <param name="isTrue"></param>
        public void Set_inDashWindow(int isTrue)
        {
            inDashWindow = Convert.ToBoolean(isTrue);
        }
        /// <summary>
        /// 设置是否为极限闪避窗口期(后)
        /// </summary>
        /// <param name="isTrue"></param>
        public void Set_inDashWindow_back(int isTrue)
        {
            inDashWindow_back = Convert.ToBoolean(isTrue);
        }
        /// <summary>
        /// 设置状态可否取消
        /// </summary>
        /// <param name="isTrue"></param>
        public void Set_canCancel(int isTrue)
        {
            canCancel = Convert.ToBoolean(isTrue);
        }
        /// <summary>
        /// 设置霸体状态
        /// </summary>
        /// <param name="isTrue"></param>
        public void Set_isUnstoppable(int isTrue)
        {
            isUnstoppable = Convert.ToBoolean(isTrue);
        }
        public void Set_isInvincible(int isTrue)
        {
            isInvincible = Convert.ToBoolean(isTrue);
        }
        /// <summary>
        /// 设置攻击框normal_1
        /// </summary>
        /// <param name="isActive"></param>
        public void Set_normal_1(int isActive)
        {
            attackRange_normal_1.SetActive(Convert.ToBoolean(isActive));
        }
        public void Set_normal_2(int isActive)
        {
            attackRange_normal_2.SetActive(Convert.ToBoolean(isActive));
        }
        public void Set_normal_3(int isActive)
        {
            attackRange_normal_3.SetActive(Convert.ToBoolean(isActive));
        }
        public void Set_sky(int isActive)
        {
            attackRange_sky.SetActive(Convert.ToBoolean(isActive));
        }
        public void Set_heavy(int isActive)
        {
            attackRange_heavy.SetActive(Convert.ToBoolean(isActive));
        }
        public void Set_skill_1_1(int isActive)
        {
            attackRange_skill_1_1.SetActive(Convert.ToBoolean(isActive));
        }
        public void Set_skill_1_2(int isActive)
        {
            attackRange_skill_1_2.SetActive(Convert.ToBoolean(isActive));
        }
        public void Set_skill_2_1(int isActive)
        {
            attackRange_skill_2_1.SetActive(Convert.ToBoolean(isActive));
        }
        public void Set_skill_2_2(int isActive)
        {
            attackRange_skill_2_2.SetActive(Convert.ToBoolean(isActive));
        }
        public void Set_skill_2_3(int isActive)
        {
            attackRange_skill_2_3.SetActive(Convert.ToBoolean(isActive));
        }
        #endregion
    }
}

/// <summary>
/// 被使用技能类型枚举
/// </summary>
public enum SkillHasUse
{
    skill_2_1,
    skill_2_2, 
    skill_2_3,
    skill_default
}