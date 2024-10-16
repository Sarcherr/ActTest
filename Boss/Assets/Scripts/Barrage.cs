using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrage : MonoBehaviour
{
    //弹幕攻击脚本
    public GameObject bulletPrefab;
    new public Transform transform;
    Rigidbody2D rigid;
    [Header("弹幕伤害")] public float barrageSpeed;
    [Header("弹幕速度")] public float barrageATK;
    [Header("发射点和中心点的x轴相对距离")] public float deltaX;
    [Header("发射点和中心点的y轴相对距离")] public float deltaY;
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
            //发射弹幕先这么写,后期听策划的
            Vector3 dir = Quaternion.identity.eulerAngles;
            bullet.transform.position += barrageSpeed * Time.deltaTime * dir;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            SendMessage("此处填玩家受击函数", barrageATK);
            Destroy(gameObject);
        }
    }
}
