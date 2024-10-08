using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    /// <summary>
    /// 单位基类(继承的类型注意初始化该基类的数据成员)
    /// </summary>
    public class Unit : MonoBehaviour
    {
        [Header("最大生命值")] public int maxHP;
        [Header("移动速度")] public float moveSpeed;

        [HideInInspector] public int faceDir;
        [HideInInspector] public int currentHP;
        [HideInInspector] public bool isGrounded;
        [HideInInspector] protected GroundSensor groundSensor;
    }
}