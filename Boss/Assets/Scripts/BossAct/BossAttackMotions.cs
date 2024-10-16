using BossAct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    public class BossAttackMotions : MonoBehaviour
    {
        [Header("Boss����1�����˺�")] public float Atk1_harm;
        [Header("Boss����2�����˺�")] public float Atk2_harm;
        [Header("Boss����3�����˺�")] public float Atk3_harm;
        [Header("Boss��Ѫ��")] public float deltaRecovery;
        BossAct.BossUpdate bossUpdate;
        BossAct.ThrowChildren throwChildren;
        Animator animator;
        //�˽ű�����Boss����������״̬,�������Ӹ�����,���ϻ�Ѫ����
        public static List<int> Index;
        void Start()
        {
            animator = GetComponent<Animator>();
        }
        public void ChooseMotion()
        {
            int AttackIndex = Random.Range(1, Index.Count + 1);
            if (AttackIndex == 1)
            {
                Attack1();
            }
            else if (AttackIndex == 2)
            {
                Attack2();
            }
            else if (AttackIndex == 3)
            {
                Attack3();
            }
            else if (AttackIndex == 4)
            {
                throwChildren.Invoke("OnThrow", 0.5f);
            }
            else if (AttackIndex == 5)
            {
                BloodRecovery();
            }
        }
        void Attack1()
        {
            animator.SetBool("atkAnim1", true);
            SendMessage("�˴�������ܻ��ĺ���,ִ������ֵ����", Atk1_harm);
        }
        void Attack2()
        {
            animator.SetBool("atkAnim2", true);
            SendMessage("�˴�������ܻ��ĺ���,ִ������ֵ����", Atk2_harm);
        }
        void Attack3()
        {
            animator.SetBool("atkAnim3", true);
            SendMessage("�˴�������ܻ��ĺ���,ִ������ֵ����", Atk3_harm);
        }
        void BloodRecovery()
        {
            animator.SetBool("HPrecovery", true);
            bossUpdate.preBhp += deltaRecovery;
        }
    }
}