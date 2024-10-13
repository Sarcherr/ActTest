using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss {
    //Boss的基本状态
    public abstract class BossState
    {
        //状态机类
        protected BossMachine bossM;
        protected GameObject BossObject;
        public BossState(BossMachine BM)
        {
            bossM = BM;
            BossObject = BM.BossObject;
        }
        public abstract void OnStart(); //初始化行为
        public abstract void OnFixedUpdate();
        public abstract void OnUpdate();
        public abstract void OnExit();   //状态结束时的行为
    }
    //Boss状态枚举
    public enum BossMode
    {
        idle,       //未侦测到玩家时左右巡逻
        chase,      //侦测到玩家后追上去
        attack,     //到达攻击半径内开始攻击
        hurt,       //正在受到攻击(?)
        shortPause, //短硬直
        longPause,  //长硬直
        dead,       //死亡
    }
    public enum BossAtk //Boss的攻击方式
    {

    }
}

