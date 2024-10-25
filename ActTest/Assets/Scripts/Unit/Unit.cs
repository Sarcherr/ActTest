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
        /// <summary>
        /// 最大生命值
        /// </summary>
        [Header("最大生命值")] public int maxHP;
        /// <summary>
        /// 移动速度
        /// </summary>
        [Header("移动速度")] public float moveSpeed;
        /// <summary>
        /// 跳跃力度
        /// </summary>
        [Header("跳跃力度")] public float jumpForce;

        /// <summary>
        /// 朝向
        /// </summary>
        [HideInInspector] public int faceDir;
        /// <summary>
        /// 当前生命值
        /// </summary>
        [HideInInspector] public int currentHP;
        /// <summary>
        /// 是否触地
        /// </summary>
        [HideInInspector] public bool isGrounded;
        /// <summary>
        /// 触底传感器
        /// </summary>
        [HideInInspector] public GroundSensor groundSensor;
    }
}