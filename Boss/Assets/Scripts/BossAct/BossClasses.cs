using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    public class BossClasses : MonoBehaviour
    {
        [Header("Boss��һ�׶�������ֵ")] public float BossHP;
        [Header("Boss�ڶ��׶�������ֵ")] public float BossHP2;
        [Header("Boss��⵽���")][HideInInspector] public bool BosscheckPlayer;
        [Header("Boss��һ�׶�������ֵ")] public float BossTough;
        [Header("Boss�ڶ��׶�������ֵ")] public float BossTough2;
        [Header("Boss���Իָ��ٶ�")] public float BossToughRecoverSpeed;
        [Header("Boss���ι���������ֵ")] public float minBossAtkTough;
        [Header("Bossת���׶�Ѫ����ֵ")] public float minBossHPchangeStage;
        [Header("Boss��Ӳֱʱ��")] public float shortBossPauseTime;
        [Header("Boss��Ӳֱʱ��")] public float longBossPauseTime;
        [Header("Boss����")][HideInInspector] public bool faceDirection;
    }
}