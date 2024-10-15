using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    /*  ���̳е����Ա:
        Boss������ֵ float BossHP
        Boss��⵽��� bool BosscheckPlayer
        Boss������ֵ float BossTough
        Boss���Իָ��ٶ� float BossToughRecoverSpeed
        Boss���ι���������ֵ float minBossAtkTough
        Bossת���׶�Ѫ����ֵ float minBossHPchangeStage
        Boss��Ӳֱʱ�� float shortBossPauseTime
        Boss��Ӳֱʱ�� float longBossPauseTime         */
    public class BossUpdate : BossClasses
    {
        [Header("Boss��ǰ����ֵ")][HideInInspector] public float preBhp;
        [Header("Boss��ǰ����ֵ")][HideInInspector] public float Btough;
        [Header("Boss�Ƿ��չ�")][HideInInspector] public bool getSimpleAtk;
        [Header("Boss�Ƿ��ػ�")][HideInInspector] public bool getHardAtk;
        [Header("Boss�Ƿ񱻼��ܹ���")][HideInInspector] public bool getSkillAtk;
        [Header("Boss������ʽ")][HideInInspector] public List<int> AtkActions = new List<int> {1, 2, 3, 4, 5};
        private Rigidbody2D rigidbody2; //Boss����
        private Boss.BossMachine bossMachine;   //Boss״̬��
        private Animator animator;  //Boss������
        private void Start()
        {
            faceDirection = true;  //�����ж�:��Ϊfalse,��Ϊtrue
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
            animator.SetLayerWeight(0, 1);  //��ʼ�趨ֻʹ�õ�һ�㶯��(����ǰ)
        }
        private void Update()
        {
            bossMachine.OnUpdate();
        }
        private void FixedUpdate()
        {
            bossMachine.OnFixedUpdate();
        }
    }
}
