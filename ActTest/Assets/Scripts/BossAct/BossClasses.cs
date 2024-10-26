using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    public class BossClasses : MonoBehaviour
    {
        [Header("Boss总生命值")] public float BossHP;
        [Header("Boss破防值上限")] public float BossTough;
        [Header("Boss短硬直时间")] public float shortBossPauseTime;
        [Header("Boss长硬直时间")] public float longBossPauseTime;
        [Header("啄击伤害")] public int BossATKHarm1;
        [Header("鸟车伤害")] public int BossATKHarm2;
        [Header("拍翅膀伤害")] public int BossATKHarm3;
    }
}