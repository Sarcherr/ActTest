using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

namespace Boss {
    //Boss��״̬��(?
    public class BossMachine
    {
        //��Boss����
        public GameObject BossObject;
        //ʵʱ״̬
        public BossState presentMode;
        //״̬��
        public Dictionary<BossMode, BossState> modeTable;
        public BossMachine(GameObject Bobject) 
        {
            BossObject = Bobject;
            modeTable = new Dictionary<BossMode, BossState>();
        }
        //����״̬
        public void SetState(BossMode bossMode)
        {
            if (modeTable.ContainsKey(bossMode))
            {
                if (presentMode != modeTable[bossMode])
                {
                    presentMode = modeTable[bossMode];
                }
            }else
            {
                Debug.Log("�޴�״̬");
            }
        }
        //���������״̬
        public void AddBossMode(BossMode bossMode, BossState bossState)
        {
            if (!modeTable.ContainsKey(bossMode))
            {
                modeTable.Add(bossMode, bossState);
            }
        }
        public void AttackMode()
        {
            if (/*��ҽ������뾶*/true)
            {
                 
            }
        }
        public void SetUp()
        {
            //��ʼ��!!
            AddBossMode(BossMode.idle, new IdleMode(this));
            AddBossMode(BossMode.chase, new ChaseMode(this));
            AddBossMode(BossMode.attack, new AttackMode(this));
            AddBossMode(BossMode.hurt, new AttackMode(this));
            AddBossMode(BossMode.shortPause, new ShortPauseMode(this));
            AddBossMode(BossMode.longPause, new LongPauseMode(this));
            AddBossMode(BossMode.dead, new LongPauseMode(this));
        }
        public void OnUpdate()
        {
            presentMode.OnUpdate();
        }
        public void OnFixedUpdate()
        {
            presentMode.OnFixedUpdate();
        }
    }
}