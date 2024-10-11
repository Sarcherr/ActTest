using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    public class BossClasses : MonoBehaviour
    {
        [Header("Boss������ֵ")] public float BossHP;
        [Header("Boss��⵽���")][HideInInspector] public bool BosscheckPlayer;
        [Header("Boss������ֵ")] public float BossTough;
        [Header("Boss���Իָ��ٶ�")] public float BossToughRecoverSpeed;
        [Header("Boss���ι���������ֵ")] public float minBossAtkTough;
        [Header("Bossת���׶�Ѫ����ֵ")] public float minBossHPchangeStage;
        [Header("Boss��Ӳֱʱ��")] public float shortBossPauseTime;
        [Header("Boss��Ӳֱʱ��")] public float longBossPauseTime;
        [Header("Boss����")][HideInInspector] public bool faceDirection;
    }
}