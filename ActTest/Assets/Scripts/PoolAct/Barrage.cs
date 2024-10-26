using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrage : MonoBehaviour
{
    //��Ļ�����ű�
    public GameObject bulletPrefab;
    new Transform transform;
    
    [Header("���������ĵ��x����Ծ���")] public float deltaX;
    [Header("���������ĵ��y����Ծ���")] public float deltaY;
    
    void Start()
    {
        transform = GetComponent<Transform>();
    }
    public void ShootFeather()
    {
        Vector3 pos = transform.position + new Vector3(deltaX, deltaY, 0);
        for (int i = 0; i <= 11; i++)
        {
            Instantiate(bulletPrefab, pos, Quaternion.Euler(0, 0, 30 * i));
            //���䵯Ļ����ôд,�������߻���
        }
    }
}