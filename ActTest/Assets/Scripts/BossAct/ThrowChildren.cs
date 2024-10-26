using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    public class ThrowChildren : MonoBehaviour
    {
        //��ʵ������С����
        [Header("˦��������ĵ��x����Ծ���")] public float deltaX;
        [Header("˦��������ĵ��y����Ծ���")] public float deltaY;
        [HideInInspector][Header("ͳ���ڳ�����������")] public int num;

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