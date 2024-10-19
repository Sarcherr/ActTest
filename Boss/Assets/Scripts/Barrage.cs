using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrage : MonoBehaviour
{
    //��Ļ�����ű�
    public GameObject bulletPrefab;
    GameObject playerObject;
    new public Transform transform;
    [Header("��Ļ�˺�")] public float barrageSpeed;
    [Header("��Ļ�ٶ�")] public float barrageATK;
    [Header("���������ĵ��x����Ծ���")] public float deltaX;
    [Header("���������ĵ��y����Ծ���")] public float deltaY;
    [Header("Boss����ҵľ���")] public float distance;
    void Start()
    {
        transform = GetComponent<Transform>();
        playerObject = GameObject.Find("Player");
    }
    void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, playerObject.transform.forward);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            SendMessage("�˴�������ܻ�����", barrageATK);
            Destroy(gameObject);
        }
    }
    public void ShootFeather()
    {
        Vector3 pos = transform.position + new Vector3(deltaX, deltaY, 0);
        for (int i = 0; i <= 11; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, pos, Quaternion.Euler(0, 0, 30 * i));
            //���䵯Ļ����ôд,�������߻���
            Vector3 dir = Quaternion.identity.eulerAngles;
            bullet.transform.position += barrageSpeed * Time.deltaTime * dir;
        }
    }
}
