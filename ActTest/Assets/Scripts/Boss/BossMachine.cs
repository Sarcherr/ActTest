using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

namespace Boss {
    //Boss的状态机(?
    public class BossMachine
    {
        //绑定Boss对象
        public GameObject BossObject;
        //实时状态
        public BossState presentMode;
        //状态表
        public Dictionary<BossMode, BossState> modeTable;
        public BossMachine(GameObject Bobject) 
        {
            BossObject = Bobject;
            modeTable = new Dictionary<BossMode, BossState>();
        }
        //设置状态
        public void SetState(BossMode bossMode)
        {
            if (modeTable.ContainsKey(bossMode))
            {
                if (presentMode != modeTable[bossMode])
                {
                    presentMode = modeTable[bossMode];
                }
            }
            else
            {
                Debug.Log("无此状态");
            }
        }
        //遍历并添加状态
        public void AddBossMode(BossMode bossMode, BossState bossState)
        {
            if (!modeTable.ContainsKey(bossMode))
            {
                modeTable.Add(bossMode, bossState);
            }
        }
        public void SetUp()
        {
            //初始化!!
            AddBossMode(BossMode.idle, new IdleMode(this));
            AddBossMode(BossMode.chase, new ChaseMode(this));
            AddBossMode(BossMode.attack, new AttackMode(this));
            AddBossMode(BossMode.hurt, new AttackMode(this));
            AddBossMode(BossMode.shortPause, new ShortPauseMode(this));
            AddBossMode(BossMode.longPause, new LongPauseMode(this));
            AddBossMode(BossMode.dead, new LongPauseMode(this));
            AddBossMode(BossMode.skillAttack, new SkillAttackMode(this));
            SetState(BossMode.idle);
            presentMode.OnStart();
        }
        public void OnUpdate()
        {
            presentMode.OnUpdate();
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("boss"+presentMode);
            }
        }
        public void OnFixedUpdate()
        {
            presentMode.OnFixedUpdate();
        }
    }
}