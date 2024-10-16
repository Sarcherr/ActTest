using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    /*  被继承的类成员:
        Boss总生命值 float BossHP
        Boss检测到玩家 bool BosscheckPlayer
        Boss总韧性值 float BossTough
        Boss韧性恢复速度 float BossToughRecoverSpeed
        Boss单次攻击韧性阈值 float minBossAtkTough
        Boss转换阶段血量阈值 float minBossHPchangeStage
        Boss短硬直时间 float shortBossPauseTime
        Boss长硬直时间 float longBossPauseTime         */
    public class BossUpdate : BossClasses
    {
        [Header("Boss当前生命值")][HideInInspector] public float preBhp;
        [Header("Boss当前韧性值")][HideInInspector] public float Btough;
        [Header("Boss是否被普攻")][HideInInspector] public bool getSimpleAtk;
        [Header("Boss是否被重击")][HideInInspector] public bool getHardAtk;
        [Header("Boss是否被技能攻击")][HideInInspector] public bool getSkillAtk;
        private Rigidbody2D rigidbody2; //Boss刚体
        private Boss.BossMachine bossMachine;   //Boss状态机
        private Animator animator;  //Boss动画机
        private void Start()
        {
            faceDirection = true;  //方向判断:右为false,左为true
            BosscheckPlayer = false;
            getSimpleAtk = false;
            getHardAtk = false;
            getSkillAtk = false;
            rigidbody2 = GetComponent<Rigidbody2D>();
            bossMachine = new Boss.BossMachine(gameObject);
            bossMachine.SetUp();
            preBhp = BossHP;
            Btough = BossTough;
            animator = GetComponent<Animator>();
            animator.SetLayerWeight(0, 1);  //初始设定只使用第一层动画(红温前)
            Instantiate(gameObject);
        }
        private void Update()
        {
            bossMachine.OnUpdate();
        }
        private void FixedUpdate()
        {
            bossMachine.OnFixedUpdate();
        }
        public void ToughRecovery()
        {
            //回复韧性值的函数
            float deltaTough = 5;   //数值后期改
            Btough += deltaTough;
        }
    }
}