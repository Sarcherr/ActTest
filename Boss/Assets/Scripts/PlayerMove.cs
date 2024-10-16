using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public float verticalSpeed = 2;
    public float jumpSpeed = 20;
    float jumpTimes = 2;
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        JumpAndFall();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            jumpTimes = 2;
        }
    }
    public void JumpAndFall()
    {
        //Jump
        bool jump = Input.GetButton("Space");   //按空格跳跃,只能二段跳
        Vector3 hpos = Vector3.up * jumpSpeed * Time.deltaTime;
        if (jump && jumpTimes > 0)
        {
            jumpTimes--;
            rigid.AddForce(hpos);
        }
        //Fall
        float Lastypos = transform.position.y;
        float Preypos = transform.position.y;
    }
}
