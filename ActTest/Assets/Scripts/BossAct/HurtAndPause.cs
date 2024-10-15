using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HurtAndPause : MonoBehaviour
{
    //如果不处于受击或硬直状态,则Boss恢复韧性值
    //韧性值归零时Boss进入长硬直(?),长硬直结束后Boss韧性值回到最大
    private bool isHurt;
    private bool isSkillHurt;
    private bool isShortPaused;
    private bool isLongPaused;
    [Header("Boss短硬直时间")] public float ShortPauseTime;
    [Header("Boss长硬直时间")] public float LongPauseTime;
    private BossAct.BossUpdate bossUpdate;
    Animator animator;
    Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public void BeHurt(float deltaHP, float deltaTough, bool isHard)
    {
        //deltaHP是玩家对Boss攻击产生的伤害,希望在玩家的攻击函数中添加SendMessage
        isHurt = true;
        if (isHard)     //只有在受到重击时才会触发受击动画
        {
            animator.SetBool("hurtAnim", isHurt);
        }
        bossUpdate.preBhp -= deltaHP;
        bossUpdate.Btough -= deltaTough;
    }
    public void BeSkillHurt(float deltaHP, float deltaTough, bool pauseTime)
    {
        //deltaHP和deltaTough由玩家技能函数决定
        isSkillHurt = true;
        animator.SetBool("skillHurt", isSkillHurt);
        bossUpdate.preBhp -= deltaHP;
        bossUpdate.Btough -= deltaTough;
        //根据传送的数据决定短硬直或长硬直
    }
    public async void ShortPause()    //短硬直
    {
        isShortPaused = true;
        animator.SetBool("PauseShortAnim", isShortPaused);
        int Stime = Convert.ToInt32(ShortPauseTime * 1000);
        await Task.Delay(Stime);
    }
    public async void LongPause()     //长硬直
    {
        isLongPaused = true;
        animator.SetBool("PauseLongAnim", isLongPaused);
        int Ltime = Convert.ToInt32(LongPauseTime * 1000);
        await Task.Delay(Ltime);
    }
    public void changeState()   //变红温的函数
    {
        animator.SetBool("红温Anim", true);
        //更换动画机Layer(变红温!)
        //变红温时关闭玩家可操作性(?)
        animator.SetLayerWeight(0, 0);
        animator.SetLayerWeight(1, 1);
    }
}
