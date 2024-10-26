using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    public class ThrowChildren : MonoBehaviour
    {
        //其实不是扔小怪兽
        [Header("甩出点和中心点的x轴相对距离")] public float deltaX;
        [Header("甩出点和中心点的y轴相对距离")] public float deltaY;
        [HideInInspector][Header("统计在场附生兽数量")] public int num;

        public GameObject prefabBaby;
        void Start()
        {
            Transform transform = GetComponent<Transform>();
            num = 0;
        }
        public void OnThrow()
        {
            Debug.Log("throwBaby");
            prefabBaby = Instantiate(prefabBaby, new Vector3(transform.position.x + deltaX, transform.position.y + deltaY, transform.position.z), Quaternion.identity);
            num++;
        }
    }
}