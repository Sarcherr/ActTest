using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    public class ThrowChildren : MonoBehaviour
    {
        [Header("甩出点和中心点的x轴相对距离")] public float deltaX;
        [Header("甩出点和中心点的y轴相对距离")] public float deltaY;
        [Header("统计在场附生兽数量")] public int num;

        //Boss甩出附生兽的函数(核心部分应当挂载在附生兽上)
        [HideInInspector] public float throwRotation;     //甩出时的角度
        public float midDis = 5;            //距离的分界线,当距离较近时角度较高,反之较低
        Vector3 targetPos;                  //记录玩家的位置,暂时认为附生兽会被扔到playerPos
        private float P_B_xdis;             //获取玩家与Boss的x距离
        private float P_B_ydis;             //获取玩家与Boss的y距离
                                            //随机的角度,Boss向左时应为负,向右时应为正
        GameObject playerObject;    //找玩家
        PlayerDetector playerDetector;
        Rigidbody2D rigid;
        public Transform prefabBaby;
        public float throwTime = 1.5f;      //从甩出到落地花费的时间
                                            //(按照我玩过的游戏)任何情况下附生兽从甩出到落地花费的时间都是一样的
        void Start()
        {
            Transform transform = GetComponent<Transform>();
            playerObject = GameObject.Find("Player");
        }
        public void OnThrow()
        {
            //获取x轴,y轴距离
            targetPos = playerObject.transform.position;    //先获取玩家位置
            P_B_xdis = targetPos.x - transform.position.x;
            P_B_ydis = transform.position.y - targetPos.y;
            playerDetector.faceDir();
            //控制甩出点
            Vector3 pos = transform.position + new Vector3(deltaX, deltaY, 0);    //确定附生兽甩出位置,通过Vector3内参数微操
            //控制甩出方向
            float g = Physics2D.gravity.magnitude;                      //获取重力值
            float speedX = P_B_xdis / throwTime;                        //计算x轴运动速度
            float speedY = 0.5f * g * throwTime - P_B_ydis / throwTime; //用运动学公式计算y轴运动初速度
            float velocity = Mathf.Sqrt(Mathf.Pow(speedX, 2) + Mathf.Pow(speedY, 2));   //计算总速度(平方和)
            Vector3 direction = Vector3.Normalize(new Vector3(speedX, speedY, 0));      //计算方向向量
            //计算甩出需要的力
            rigid.AddForce(direction * velocity, ForceMode2D.Impulse);
            //附生兽出现,发射!
            Quaternion quaternion = Quaternion.Euler(direction.x, direction.y, direction.z);
            Transform baby = Instantiate(prefabBaby, pos, quaternion);
            num++;
        }
    }
}