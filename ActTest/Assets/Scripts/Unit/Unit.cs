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
        /// <summary>
        /// �������ֵ
        /// </summary>
        [Header("�������ֵ")] public int maxHP;
        /// <summary>
        /// �ƶ��ٶ�
        /// </summary>
        [Header("�ƶ��ٶ�")] public float moveSpeed;
        /// <summary>
        /// ��Ծ����
        /// </summary>
        [Header("��Ծ����")] public float jumpForce;

        /// <summary>
        /// ����
        /// </summary>
        [HideInInspector] public int faceDir;
        /// <summary>
        /// ��ǰ����ֵ
        /// </summary>
        [HideInInspector] public int currentHP;
        /// <summary>
        /// �Ƿ񴥵�
        /// </summary>
        [HideInInspector] public bool isGrounded;
        /// <summary>
        /// ���״�����
        /// </summary>
        [HideInInspector] public GroundSensor groundSensor;
    }
}