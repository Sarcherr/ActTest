using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    /// <summary>
    /// ��λ����(�̳е�����ע���ʼ���û�������ݳ�Ա)
    /// </summary>
    public class Unit : MonoBehaviour
    {
        [Header("�������ֵ")] public int maxHP;
        [Header("�ƶ��ٶ�")] public float moveSpeed;

        [HideInInspector] public int faceDir;
        [HideInInspector] public int currentHP;
        [HideInInspector] public bool isGrounded;
        [HideInInspector] protected GroundSensor groundSensor;
    }
}