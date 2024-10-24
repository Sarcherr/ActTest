using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BossAct
{
    public class HurtAndPause : MonoBehaviour
    {
        //如果不处于受击或硬直状态,则Boss恢复韧性值
        //韧性值归零时Boss进入长硬直(?),长硬直结束后Boss韧性值回到最大
        [HideInInspector] public bool isHurt;
        [HideInInspector] public bool isHarded;
        [HideInInspector] public bool isSkillHurt;
        [HideInInspector] public bool isShortPaused;
        [HideInInspector] public bool isLongPaused;
        [HideInInspector][Header("玩家攻击扣除的血量")] public float minusHP;
        [HideInInspector][Header("玩家攻击扣除的韧性值")] public float minusTough;
        [Header("Boss短硬直时间")] public float ShortPauseTime;
        [Header("Boss长硬直时间")] public float LongPauseTime;
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
            //deltaHP是玩家对Boss攻击产生的伤害,希望在玩家的攻击函数中添加SendMessage
            isHurt = true; animator.SetBool("isHurt", isHurt);
            if (isHard)     //只有在受到重击时才会触发受击动画
            {
                animator.SetInteger("isPause", 1);
            }
            bossUpdate.preBhp -= deltaHP;
            bossUpdate.Btough -= deltaTough;
            isHurt = false; animator.SetBool("isHurt", isHurt);
        }
        public void BeSkillHurt(float deltaHP, float deltaTough)
        {
            //deltaHP和deltaTough由玩家技能函数决定
            isSkillHurt = true;
            animator.SetInteger("isPause", 1);
            bossUpdate.preBhp -= deltaHP;
            bossUpdate.Btough -= deltaTough;
            //根据传送的数据决定短硬直或长硬直
        }
        public async void ShortPause()    //短硬直
        {
            animator.SetInteger("isPause", 1);
            isShortPaused = true;
            animator.SetTrigger("isShortPause");
            animName = "PauseShortAnim";
            int Stime = Convert.ToInt32(ShortPauseTime * 1000);
            await Task.Delay(Stime);
        }
        public async void LongPause()     //长硬直
        {
            animator.SetInteger("isPause", 1);
            isLongPaused = true;
            animator.SetTrigger("isLongPause");
            animName = "PauseLongAnim";
            int Ltime = Convert.ToInt32(LongPauseTime * 1000);
            await Task.Delay(Ltime);
        }
        public void changeState()   //变红温的函数
        {
            animator.SetTrigger("TurnState");
            animName = "红温Anim";
            //更换动画机Layer(变红温!)
            //变红温时关闭玩家可操作性(?)
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
            //然后爆出食材?
        }
    }
}