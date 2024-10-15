using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss {
    //Boss�Ļ���״̬
    public abstract class BossState
    {
        //״̬����
        protected BossMachine bossM;
        protected GameObject BossObject;
        protected BossAct.BossUpdate bossUpdate;
        public BossState(BossMachine BM)
        {
            bossM = BM;
            BossObject = BM.BossObject;
        }
        public abstract void OnStart(); //��ʼ����Ϊ
        public abstract void OnFixedUpdate();
        public abstract void OnUpdate();
        public abstract void OnExit();   //״̬����ʱ����Ϊ
    }
    //Boss״̬ö��
    public enum BossMode
    {
        idle,           //δ��⵽���ʱ����Ѳ��
        chase,          //��⵽��Һ�׷��ȥ
        attack1,        //���﹥���뾶�ڿ�ʼ����
        attack2,
        attack3,
        throwchildren,  //�ӳ�������
        hurt,           //�����ܵ�����(?)
        shortPause,     //��Ӳֱ
        longPause,      //��Ӳֱ
        dead,           //����
    }
    public enum BossAtk //Boss�Ĺ�����ʽ
    {

    }
}