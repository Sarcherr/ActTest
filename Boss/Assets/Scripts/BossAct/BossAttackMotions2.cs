using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    public class BossAttackMotions2 : MonoBehaviour
    {
        //�˽ű�����Boss�ļ��ܹ���(�ٻ�����ë����)
        ThrowChildren throwBaby;
        Barrage shootFeather;
        Animator animator;

        [Header("Boss�Ӹ����޵��ӳ�ʱ��")] public float restTime;
        [Header("Boss�ͷ���ë�ľ�����ֵ")] public float shootDis;
        void Start ()
        {
            shootFeather = GetComponent<Barrage>();
            throwBaby = GetComponent<ThrowChildren>();
        }
        public void FixedUpdate()
        {
            int i = Random.Range(0, 2); //i=0ʱ������ë,i=1ʱ�Ӹ�����
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