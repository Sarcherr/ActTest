using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    public class BossClasses : MonoBehaviour
    {
        [Header("Boss������ֵ")] public float BossHP;
        [Header("Boss�Ʒ�ֵ����")] public float BossTough;
        [Header("Boss��Ӳֱʱ��")] public float shortBossPauseTime;
        [Header("Boss��Ӳֱʱ��")] public float longBossPauseTime;
        [Header("�Ļ��˺�")] public int BossATKHarm1;
        [Header("���˺�")] public int BossATKHarm2;
        [Header("�ĳ���˺�")] public int BossATKHarm3;
    }
}