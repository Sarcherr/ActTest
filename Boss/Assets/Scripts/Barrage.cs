using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrage : MonoBehaviour
{
    //��Ļ�����ű�
    public GameObject bulletPrefab;
    new public Transform transform;
    Rigidbody2D rigid;
    [Header("��Ļ�˺�")] public float barrageSpeed;
    [Header("��Ļ�ٶ�")] public float barrageATK;
    [Header("���������ĵ��x����Ծ���")] public float deltaX;
    [Header("���������ĵ��y����Ծ���")] public float deltaY;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        Vector3 pos = transform.position + new Vector3(deltaX, deltaY, 0);
        for (int i = 1; i <= 12; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, pos, Quaternion.Euler(0, 0, 30 * i));
            //���䵯Ļ����ôд,�������߻���
            Vector3 dir = Quaternion.identity.eulerAngles;
            bullet.transform.position += barrageSpeed * Time.deltaTime * dir;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            SendMessage("�˴�������ܻ�����", barrageATK);
            Destroy(gameObject);
        }
    }
}
