using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class IdleMode : BossState
    {
        PlayerDetector playerDetector;
        Animator animator;
        public IdleMode(BossMachine BM) : base(BM)
        {
            
        }
        public override void OnStart()
        {
            playerDetector.Search();
        }
        public override void OnFixedUpdate()
        {
            playerDetector.Search();
        }
        public override void OnExit()
        {
            playerDetector.ChaseR();
        }
        public override void OnUpdate()
        {
            
        }
    }
    public class ChaseMode : BossState
    {
        PlayerDetector playerDetector;
        public ChaseMode(BossMachine BM) : base(BM)
        {

        }
        public override void OnStart()
        {
            playerDetector.ChaseR();
        }
        public override void OnFixedUpdate()
        {
            playerDetector.ChaseR();
        }
        public override void OnExit()
        {
            playerDetector.AttackR();
        }
        public override void OnUpdate()
        {
            
        }
    }
    public class AttackMode : BossState
    {
        PlayerDetector playerDetector;
        public AttackMode(BossMachine BM) : base(BM)
        {

        }
        public override void OnStart()
        {
            playerDetector.AttackR();
            //后期要添加攻击动作函数
        }
        public override void OnFixedUpdate()
        {

        }
        public override void OnExit()
        {
            //血量到达阈值时切换阶段
        }
        public override void OnUpdate()
        {

        }
    }
    public class HurtMode : BossState
    {
        public HurtMode(BossMachine BM) : base(BM)
        {

        }
        public override void OnStart()
        {
            
        }
        public override void OnFixedUpdate()
        {

        }
        public override void OnExit()
        {
            
        }
        public override void OnUpdate()
        {

        }
    }
    public class ShortPauseMode : BossState
    {
        public ShortPauseMode(BossMachine BM) : base(BM)
        {

        }
        public override void OnStart()
        {
            
        }
        public override void OnFixedUpdate()
        {

        }
        public override void OnExit()
        {
            
        }
        public override void OnUpdate()
        {

        }
    }
    public class LongPauseMode : BossState
    {
        public LongPauseMode(BossMachine BM) : base(BM)
        {

        }
        public override void OnStart()
        {
            
        }
        public override void OnFixedUpdate()
        {

        }
        public override void OnExit()
        {
            
        }
        public override void OnUpdate()
        {

        }
    }
    public class DeadMode : BossState
    {
        public DeadMode(BossMachine BM) : base(BM)
        {

        }
        public override void OnStart()
        {
            
        }
        public override void OnFixedUpdate()
        {

        }
        public override void OnExit()
        {

        }
        public override void OnUpdate()
        {

        }
    }
}