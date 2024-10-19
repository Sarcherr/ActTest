using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    public class BossAttackMotions2 : MonoBehaviour
    {
        //此脚本关于Boss的技能攻击(召唤和羽毛齐射)
        ThrowChildren throwBaby;
        Barrage shootFeather;
        Animator animator;

        [Header("Boss扔附生兽的延迟时间")] public float restTime;
        [Header("Boss释放羽毛的距离阈值")] public float shootDis;
        void Start ()
        {
            shootFeather = GetComponent<Barrage>();
            throwBaby = GetComponent<ThrowChildren>();
        }
        public void FixedUpdate()
        {
            int i = Random.Range(0, 2); //i=0时发射羽毛,i=1时扔附生兽
            if (i == 0)
            {
                int babynum = throwBaby.num;
                if (babynum <= 1)
                {
                    animator.SetTrigger("throwBaby");
                    Invoke(nameof(throwBaby.OnThrow), restTime);
                }
                else if (babynum > 1)
                {
                    return;
                }
            }
            else if (i == 1)
            {
                if (shootDis >= shootFeather.distance)
                {
                    animator.SetTrigger("shootFeather");
                    shootFeather.ShootFeather();
                }
                else if (shootDis < shootFeather.distance)
                {
                    return;
                }
            }
        }
    }
}