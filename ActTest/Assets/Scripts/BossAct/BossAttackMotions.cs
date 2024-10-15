using BossAct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    public class BossAttackMotions : MonoBehaviour
    {
        [Header("Boss攻击1产生伤害")] public float Atk1_harm;
        [Header("Boss攻击2产生伤害")] public float Atk2_harm;
        [Header("Boss攻击3产生伤害")] public float Atk3_harm;
        [Header("Boss回血量")] public float deltaRecovery;
        BossAct.BossUpdate bossUpdate;
        BossAct.ThrowChildren throwChildren;
        Animator animator;
        //此脚本关于Boss的三个攻击状态,并引用扔附生兽,加上回血操作
        public List<int> Index;
        void Start()
        {
            animator = GetComponent<Animator>();
            Index = bossUpdate.AtkActions;
        }
        public void ChooseMotion()
        {
            int AttackIndex = Random.Range(1, Index.Count + 1);
            if (AttackIndex == Index[1])
            {
                Attack1();
            }
            else if (AttackIndex == Index[2])
            {
                Attack2();
            }
            else if (AttackIndex == Index[3])
            {
                Attack3();
            }
            else if (AttackIndex == Index[4])
            {
                throwChildren.Invoke("OnThrow", 0.5f);
            }
            else if (AttackIndex == Index[5])
            {
                Recovery();
            }
        }
        void Attack1()
        {
            animator.SetBool("atkAnim1", true);
            SendMessage("此处填玩家受击的函数,执行生命值减少", Atk1_harm);
        }
        void Attack2()
        {
            animator.SetBool("atkAnim2", true);
            SendMessage("此处填玩家受击的函数,执行生命值减少", Atk2_harm);
        }
        void Attack3()
        {
            animator.SetBool("atkAnim3", true);
            SendMessage("此处填玩家受击的函数,执行生命值减少", Atk3_harm);
        }
        void Recovery()
        {
            animator.SetBool("HPrecovery", true);
            bossUpdate.preBhp += deltaRecovery;
        }
    }
}