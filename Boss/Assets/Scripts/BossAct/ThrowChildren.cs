using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAct
{
    public class ThrowChildren : MonoBehaviour
    {
        [Header("˦��������ĵ��x����Ծ���")] public float deltaX;
        [Header("˦��������ĵ��y����Ծ���")] public float deltaY;
        [Header("ͳ���ڳ�����������")] public int num;

        //Boss˦�������޵ĺ���(���Ĳ���Ӧ�������ڸ�������)
        [HideInInspector] public float throwRotation;     //˦��ʱ�ĽǶ�
        public float midDis = 5;            //����ķֽ���,������Ͻ�ʱ�ǶȽϸ�,��֮�ϵ�
        Vector3 targetPos;                  //��¼��ҵ�λ��,��ʱ��Ϊ�����޻ᱻ�ӵ�playerPos
        private float P_B_xdis;             //��ȡ�����Boss��x����
        private float P_B_ydis;             //��ȡ�����Boss��y����
                                            //����ĽǶ�,Boss����ʱӦΪ��,����ʱӦΪ��
        GameObject playerObject;    //�����
        PlayerDetector playerDetector;
        Rigidbody2D rigid;
        public Transform prefabBaby;
        public float throwTime = 1.5f;      //��˦������ػ��ѵ�ʱ��
                                            //(�������������Ϸ)�κ�����¸����޴�˦������ػ��ѵ�ʱ�䶼��һ����
        void Start()
        {
            Transform transform = GetComponent<Transform>();
            playerObject = GameObject.Find("Player");
        }
        public void OnThrow()
        {
            //��ȡx��,y�����
            targetPos = playerObject.transform.position;    //�Ȼ�ȡ���λ��
            P_B_xdis = targetPos.x - transform.position.x;
            P_B_ydis = transform.position.y - targetPos.y;
            playerDetector.faceDir();
            //����˦����
            Vector3 pos = transform.position + new Vector3(deltaX, deltaY, 0);    //ȷ��������˦��λ��,ͨ��Vector3�ڲ���΢��
            //����˦������
            float g = Physics2D.gravity.magnitude;                      //��ȡ����ֵ
            float speedX = P_B_xdis / throwTime;                        //����x���˶��ٶ�
            float speedY = 0.5f * g * throwTime - P_B_ydis / throwTime; //���˶�ѧ��ʽ����y���˶����ٶ�
            float velocity = Mathf.Sqrt(Mathf.Pow(speedX, 2) + Mathf.Pow(speedY, 2));   //�������ٶ�(ƽ����)
            Vector3 direction = Vector3.Normalize(new Vector3(speedX, speedY, 0));      //���㷽������
            //����˦����Ҫ����
            rigid.AddForce(direction * velocity, ForceMode2D.Impulse);
            //�����޳���,����!
            Quaternion quaternion = Quaternion.Euler(direction.x, direction.y, direction.z);
            Transform baby = Instantiate(prefabBaby, pos, quaternion);
            num++;
        }
    }
}