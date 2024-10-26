using BossAct;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Boss
{
    public class IdleMode : BossState
    {
        public Rigidbody2D rigid;
        public IdleMode(BossMachine BM) : base(BM)
        {
            rigid = theBoss.GetComponent<Rigidbody2D>();
        }
        public override void OnStart()
        {
            theBoss.isIdle = true;
            Debug.Log(BossMode.idle);
        }
        public override void OnFixedUpdate()
        {
            theBoss.Idle();
            theBoss.CheckState();
        }
        public override void OnDestroy()
        {
            theBoss.isIdle = false;
            theBoss.isChase = true;
        }
        public override void OnUpdate()
        {
            if (theBoss.isIdle)
            {
                theBoss.animator.Play("Boss_Idle");
            }
            else if (!theBoss.isIdle)
            {
                bossM.SetState(BossMode.chase);
                OnDestroy();
                bossM.presentMode.OnStart();
            }
        }
    }
    public class ChaseMode : BossState
    {
        public ChaseMode(BossMachine BM) : base(BM)
        {
            
        }
        public override void OnStart()
        {
            theBoss.isChase = true;
            theBoss.faceDir();
            Debug.Log(BossMode.chase);
        }
        public override void OnFixedUpdate()
        {
            theBoss.ChaseR();
            theBoss.CheckState();
        }
        public override void OnDestroy()
        {
            theBoss.isChase = false;
            theBoss.isATK = true;
        }
        public override void OnUpdate()
        {
            if (theBoss.isChase)
            {
                theBoss.animator.Play("Boss_Chase");
            }
            else if (!theBoss.isChase)
            {
                bossM.SetState(BossMode.attack);
                OnDestroy();
                bossM.presentMode.OnStart();
            }
            theBoss.changeState();
        }
    }
    public class AttackMode : BossState
    {
        public Rigidbody2D rigid;
        public AttackMode(BossMachine BM) : base(BM)
        {
            rigid = theBoss.GetComponent<Rigidbody2D>();
        }
        public override void OnStart()
        {
            theBoss.isATK = true;
            Debug.Log(BossMode.attack);
            theBoss.AttackIndex = Random.Range(1, 4);
            switch (theBoss.AttackIndex)
            {
                case 1:
                    {
                        theBoss.Attack1();
                        theBoss.animator.Play("Boss_atk1");
                        break;
                    }
                case 2:
                    {
                        theBoss.animator.Play("Boss_atk2");
                        break;
                    }
                case 3:
                    {
                        theBoss.Attack3();
                        theBoss.animator.Play("Boss_atk3");
                        break;
                    }
            }
        }
        public override void OnFixedUpdate()
        {
            //theBoss.CheckState();
        }
        public override void OnDestroy()
        {
            theBoss.attackTimer = theBoss.attackCD;
            theBoss.isATK = false;  
            theBoss.AttackIndex = 0;
            if (theBoss.isLongPaused)
            {
                bossM.SetState(BossMode.longPause);
                bossM.presentMode.OnStart();
            }
            else if (theBoss.isShortPaused)
            {
                bossM.SetState(BossMode.shortPause);
                bossM.presentMode.OnStart();
            }
            if (!theBoss.isHurt)
            {
                bossM.SetState(BossMode.chase);
                bossM.presentMode.OnStart();
            }
        }
        public override void OnUpdate()
        { 
            theBoss.changeState();
        }
    }
    public class SkillAttackMode : BossState
    {
        Rigidbody2D rigid;
        public float timer;
        public SkillAttackMode(BossMachine BM) : base(BM)
        {
            rigid = theBoss.GetComponent<Rigidbody2D>();
        }
        public override void OnStart()
        {
            theBoss.DecideSkillAtk();
            Debug.Log(BossMode.skillAttack);
            timer = 0;
        }
        public override void OnFixedUpdate()
        {
            
        }
        public override void OnDestroy()
        {
            theBoss.isATK = false;
            theBoss.pretimerTime = theBoss.SkillCD2;
        }
        public override void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                bossM.SetState(BossMode.chase);
                OnDestroy();
                bossM.presentMode.OnStart();
            }
            if (theBoss.isLongPaused)
            {
                bossM.SetState(BossMode.longPause);
                OnDestroy();
                bossM.presentMode.OnStart();
            }
            else if (theBoss.isShortPaused)
            {
                bossM.SetState(BossMode.shortPause);
                OnDestroy();
                bossM.presentMode.OnStart();
            }
            theBoss.changeState();
        }
    }
    public class HurtMode : BossState
    {
        Rigidbody2D rigid;
        public HurtMode(BossMachine BM) : base(BM)
        {
            rigid = theBoss.GetComponent<Rigidbody2D>();
        }
        public override void OnStart()
        {
            Debug.Log(BossMode.hurt);
        }
        public override void OnFixedUpdate()
        {

        }
        public override void OnDestroy()
        {
            theBoss.isHurt = false;
        }
        public override void OnUpdate()
        {
            if (theBoss.isLongPaused)
            {
                bossM.SetState(BossMode.longPause);
                OnDestroy();
                bossM.presentMode.OnStart();
            }
            else if (theBoss.isShortPaused)
            {
                bossM.SetState(BossMode.shortPause);
                OnDestroy();
                bossM.presentMode.OnStart();
            }
            theBoss.changeState();
        }
    }
    public class ShortPauseMode : BossState
    {
        public ShortPauseMode(BossMachine BM) : base(BM)
        {

        }
        public override void OnStart()
        {
            BossObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            theBoss.animator.Play("Boss_ShortPause");
            OnDestroy();
        }
        public override void OnFixedUpdate()
        {

        }
        public override void OnDestroy()
        {
            theBoss.isShortPaused = false;
        }
        public override void OnUpdate()
        {
            theBoss.changeState();
        }
    }
    public class LongPauseMode : BossState
    {
        public LongPauseMode(BossMachine BM) : base(BM)
        {

        }
        public override void OnStart()
        {
            BossObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            theBoss.animator.Play("Boss_LongPause");
            OnDestroy();
        }
        public override void OnFixedUpdate()
        {

        }
        public override void OnDestroy()
        {
            theBoss.isLongPaused = false;
        }
        public override void OnUpdate()
        {
            theBoss.changeState();
        }
    }
    public class DeadMode : BossState
    {
        GameObject gameobject;
        public DeadMode(BossMachine BM) : base(BM)
        {
            gameobject = theBoss.GetComponent<GameObject>();
        }
        public override void OnStart()
        {
            theBoss.Dead();
            theBoss.animator.Play("Boss_Dead");
            OnDestroy();
        }
        public override void OnFixedUpdate()
        {

        }
        public override void OnDestroy()
        {
            gameobject.SetActive(false);
        }
        public override void OnUpdate()
        {

        }
    }
}