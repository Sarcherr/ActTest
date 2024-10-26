using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrage : MonoBehaviour
{
    //弹幕攻击脚本
    public GameObject bulletPrefab;
    new Transform transform;
    
    [Header("发射点和中心点的x轴相对距离")] public float deltaX;
    [Header("发射点和中心点的y轴相对距离")] public float deltaY;
    
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
            //发射弹幕先这么写,后期听策划的
        }
    }
}