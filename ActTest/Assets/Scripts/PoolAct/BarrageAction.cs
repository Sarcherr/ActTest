using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;

public class BarrageAction : MonoBehaviour
{
    GameObject playerObject;
    Animator animator;
    Player player;
    Barrage barrage;
    [Header("µ¯Ä»ËÙ¶È")] public float barrageSpeed;
    [Header("µ¯Ä»ÉËº¦")] public int barrageATK;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public float timer;
    private void Start()
    {
        animator = GetComponent<Animator>();
        barrage = GetComponent<Barrage>();
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        dir = new Vector3(-Mathf.Cos(angle), -Mathf.Sin(angle), 0);
        timer = 0;
    }
    void FixedUpdate()
    {
        transform.position += barrageSpeed * Time.deltaTime * dir;
        animator.Play("Feather_Shoot");
        if (timer < 2)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            animator.SetBool("OnHit", true);
            collision.GetComponent<Player>().Hurt(barrageATK);
            gameObject.SetActive(false);
        }
    }
}