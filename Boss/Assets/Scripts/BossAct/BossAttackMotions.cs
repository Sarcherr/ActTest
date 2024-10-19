using BossAct;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BossAct
{
    public class BossAttackMotions : MonoBehaviour
    {
        //此脚本关于Boss的常规攻击
        //关于鸟车移动和翅膀拍击时旋转的代码,如果动画中包含组件动作就删掉
        [Header("Boss啄击产生伤害")] public float Atk1_harm;
        [Header("Boss鸟车产生伤害")] public float Atk2_harm;
        [Header("Boss鸟车冲刺速度")] public float rushSpeed;
        [Header("Boss鸟车冲刺时间")] public float rushTime;
        [Header("Boss翅膀拍击产生伤害")] public float Atk3_harm;
        [Header("Boss拍击时翅膀旋转速度")] public float wingRotateSpeed;
        [HideInInspector] public int AttackIndex;
        BossUpdate bossUpdate;
        PlayerDetector playerDetector;
        ThrowChildren throwChildren;
        Animator animator;
        Transform[] children;
        private GameObject leftWing;
        private GameObject rightWing;

        void Start()
        {
            animator = GetComponent<Animator>();
            bossUpdate = GetComponent<BossUpdate>();
            playerDetector = GetComponent<PlayerDetector>();
            throwChildren = GetComponent<ThrowChildren>();
        }
        public void FixedUpdate()
        {
            AttackIndex = Random.Range(1, 4);
            if (AttackIndex == 1)       //啄击
            {
                animator.SetBool("isAttack", true);
                Attack1();
                animator.SetInteger("AttackIndex", 0);
            }
            else if (AttackIndex == 2)  //鸟车
            {
                animator.SetBool("isAttack", true);
                Attack2();
                animator.SetInteger("AttackIndex", 0);
            }
            else if (AttackIndex == 3)  //翅膀拍击
            {
                animator.SetBool("isAttack", true);
                Attack3();
                animator.SetInteger("AttackIndex", 0);
            }
        }        
        void Attack1()
        {
            animator.SetInteger("AttackIndex", 1);
        }
        void Attack2()
        {
            animator.SetInteger("AttackIndex", 2);
            float presentTime = Time.time;
            if (Time.time < presentTime + rushTime)
            {
                return;
            }
            else
            {
                presentTime = Time.time;
            }
            transform.position += playerDetector.vecDir * rushSpeed * Time.deltaTime;
            presentTime = Time.time;
        }
        void Attack3()
        {
            animator.SetInteger("AttackIndex", 3);
            foreach (var child in children)
            {
                //获取两翅膀
                if (child.name == "LeftWing")
                {
                    leftWing = child.gameObject;
                }
                if (child.name == "RightWing")
                {
                    rightWing = child.gameObject;
                }
                //旋转翅膀(向上时左翅膀z轴负方向转,右翅膀z轴正方向转)
                float minZRotation = -45;
                float maxZRotation = 45;
                float preRotation = leftWing.transform.rotation.z;
                bool rotateDir = true;  //翅膀拍动方向,true向上
                if (preRotation < minZRotation)
                {
                    preRotation = minZRotation;
                    rotateDir = !rotateDir;
                }
                else if (preRotation > maxZRotation)
                {
                    preRotation = maxZRotation;
                    rotateDir = !rotateDir;
                }
                if (rotateDir)
                {
                    preRotation -= wingRotateSpeed * Time.deltaTime;
                    leftWing.transform.rotation = Quaternion.Euler(0, 0, preRotation);
                    rightWing.transform.rotation = Quaternion.Euler(0, 0, -preRotation);
                }
                else if (!rotateDir)
                {
                    preRotation += wingRotateSpeed * Time.deltaTime;
                    leftWing.transform.rotation = Quaternion.Euler(0, 0, preRotation);
                    rightWing.transform.rotation = Quaternion.Euler(0, 0, -preRotation);
                }
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            children = GetComponentsInChildren<Transform>();
            Collider2D playercollider = collision.otherCollider;
            if (AttackIndex == 1)
            {
                foreach (var child in children)
                {
                    if (child.name == "喙子物体的名字" && collision.gameObject.name == "Player" && child.gameObject == gameObject)
                    {
                        SendMessage("此处填玩家受击的函数,执行生命值减少", Atk1_harm);
                    }
                }
            }
            else if(AttackIndex == 2)
            {
                if(collision.gameObject.name == "Player")
                {
                    SendMessage("此处填玩家受击的函数,执行生命值减少", Atk2_harm);
                }
            }
            else if (AttackIndex == 3)
            {
                if (collision.gameObject.name == "Player")
                {
                    SendMessage("此处填玩家受击的函数,执行生命值减少", Atk3_harm);
                }
            }
        }
    }
}