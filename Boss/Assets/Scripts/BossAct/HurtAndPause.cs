using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BossAct
{
    public class HurtAndPause : MonoBehaviour
    {
        //����������ܻ���Ӳֱ״̬,��Boss�ָ�����ֵ
        //����ֵ����ʱBoss���볤Ӳֱ(?),��Ӳֱ������Boss����ֵ�ص����
        [HideInInspector] public bool isHurt;
        [HideInInspector] public bool isHarded;
        [HideInInspector] public bool isSkillHurt;
        [HideInInspector] public bool isShortPaused;
        [HideInInspector] public bool isLongPaused;
        [HideInInspector][Header("��ҹ����۳���Ѫ��")] public float minusHP;
        [HideInInspector][Header("��ҹ����۳�������ֵ")] public float minusTough;
        [Header("Boss��Ӳֱʱ��")] public float ShortPauseTime;
        [Header("Boss��Ӳֱʱ��")] public float LongPauseTime;
        public string animName;
        private BossAct.BossUpdate bossUpdate;
        PlayerDetector playerDetector;
        Animator animator;
        void Start()
        {
            animator = GetComponent<Animator>();
            bossUpdate = GetComponent<BossUpdate>();
            playerDetector = GetComponent<PlayerDetector>();
        }
        private void FixedUpdate()
        {
            animator.SetInteger("isPause", 0);
            if (bossUpdate.preBhp < bossUpdate.BossHP * 0.6f)
            {
                changeState();
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name == "PlayerWeapon")
            {
                if (isSkillHurt)
                {
                    BeSkillHurt(minusHP, minusTough);
                }
                else
                {
                    BeHurt(minusHP, minusTough, isHarded);
                }
            }
        }
        public void BeHurt(float deltaHP, float deltaTough, bool isHard)
        {
            //deltaHP����Ҷ�Boss�����������˺�,ϣ������ҵĹ������������SendMessage
            isHurt = true; animator.SetBool("isHurt", isHurt);
            if (isHard)     //ֻ�����ܵ��ػ�ʱ�Żᴥ���ܻ�����
            {
                animator.SetInteger("isPause", 1);
            }
            bossUpdate.preBhp -= deltaHP;
            bossUpdate.Btough -= deltaTough;
            isHurt = false; animator.SetBool("isHurt", isHurt);
        }
        public void BeSkillHurt(float deltaHP, float deltaTough)
        {
            //deltaHP��deltaTough����Ҽ��ܺ�������
            isSkillHurt = true;
            animator.SetInteger("isPause", 1);
            bossUpdate.preBhp -= deltaHP;
            bossUpdate.Btough -= deltaTough;
            //���ݴ��͵����ݾ�����Ӳֱ��Ӳֱ
        }
        public async void ShortPause()    //��Ӳֱ
        {
            animator.SetInteger("isPause", 1);
            isShortPaused = true;
            animator.SetTrigger("isShortPause");
            animName = "PauseShortAnim";
            int Stime = Convert.ToInt32(ShortPauseTime * 1000);
            await Task.Delay(Stime);
        }
        public async void LongPause()     //��Ӳֱ
        {
            animator.SetInteger("isPause", 1);
            isLongPaused = true;
            animator.SetTrigger("isLongPause");
            animName = "PauseLongAnim";
            int Ltime = Convert.ToInt32(LongPauseTime * 1000);
            await Task.Delay(Ltime);
        }
        public void changeState()   //����µĺ���
        {
            animator.SetTrigger("TurnState");
            animName = "����Anim";
            //����������Layer(�����!)
            //�����ʱ�ر���ҿɲ�����(?)
            animator.SetLayerWeight(0, 0);
            animator.SetLayerWeight(1, 1);
            bossUpdate.preBhp = bossUpdate.BossHP2;
            bossUpdate.Btough = bossUpdate.BossTough2;
            playerDetector.isState = true;
        }
        public void Dead()
        {
            animator.SetTrigger("isDead");
            Destroy(gameObject);
            //Ȼ�󱬳�ʳ��?
        }
    }
}