using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    public class BossClasses : MonoBehaviour
    {
        [Header("Boss总生命值")] public float BossHP;
        [Header("Boss检测到玩家")][HideInInspector] public bool BosscheckPlayer;
        [Header("Boss总韧性值")] public float BossTough;
        [Header("Boss韧性恢复速度")] public float BossToughRecoverSpeed;
        [Header("Boss单次攻击韧性阈值")] public float minBossAtkTough;
        [Header("Boss转换阶段血量阈值")] public float minBossHPchangeStage;
        [Header("Boss短硬直时间")] public float shortBossPauseTime;
        [Header("Boss长硬直时间")] public float longBossPauseTime;
        [Header("Boss方向")][HideInInspector] public bool faceDirection;
    }
}