using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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
    private BossAct.BossUpdate bossUpdate;
    Animator animator;
    Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bossUpdate = GetComponent<BossAct.BossUpdate>();
    }
    public void ReceiveMessage()
    {

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
        isHurt = true;
        if (isHard)     //ֻ�����ܵ��ػ�ʱ�Żᴥ���ܻ�����
        {
            animator.SetBool("hurtAnim", isHurt);
        }
        bossUpdate.preBhp -= deltaHP;
        bossUpdate.Btough -= deltaTough;
    }
    public void BeSkillHurt(float deltaHP, float deltaTough)
    {
        //deltaHP��deltaTough����Ҽ��ܺ�������
        isSkillHurt = true;
        animator.SetBool("skillHurt", isSkillHurt);
        bossUpdate.preBhp -= deltaHP;
        bossUpdate.Btough -= deltaTough;
        //���ݴ��͵����ݾ�����Ӳֱ��Ӳֱ
    }
    public async void ShortPause()    //��Ӳֱ
    {
        isShortPaused = true;
        animator.SetBool("PauseShortAnim", isShortPaused);
        int Stime = Convert.ToInt32(ShortPauseTime * 1000);
        await Task.Delay(Stime);
    }
    public async void LongPause()     //��Ӳֱ
    {
        isLongPaused = true;
        animator.SetBool("PauseLongAnim", isLongPaused);
        int Ltime = Convert.ToInt32(LongPauseTime * 1000);
        await Task.Delay(Ltime);
    }
    public void changeState()   //����µĺ���
    {
        animator.SetBool("����Anim", true);
        //����������Layer(�����!)
        //�����ʱ�ر���ҿɲ�����(?)
        animator.SetLayerWeight(0, 0);
        animator.SetLayerWeight(1, 1);
        bossUpdate.preBhp = bossUpdate.BossHP2;
        bossUpdate.Btough = bossUpdate.BossTough2;
    }
    public void Dead()
    {
        animator.SetBool("DeadAnim", true);
        Destroy(gameObject);
        //Ȼ�󱬳�ʳ��?
    }
}