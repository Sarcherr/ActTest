using BossAct;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BossAct
{
    public class BossAttackMotions : MonoBehaviour
    {
        //�˽ű�����Boss�ĳ��湥��
        //�������ƶ��ͳ���Ļ�ʱ��ת�Ĵ���,��������а������������ɾ��
        [Header("Boss�Ļ������˺�")] public float Atk1_harm;
        [Header("Boss�񳵲����˺�")] public float Atk2_harm;
        [Header("Boss�񳵳���ٶ�")] public float rushSpeed;
        [Header("Boss�񳵳��ʱ��")] public float rushTime;
        [Header("Boss����Ļ������˺�")] public float Atk3_harm;
        [Header("Boss�Ļ�ʱ�����ת�ٶ�")] public float wingRotateSpeed;
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
            if (AttackIndex == 1)       //�Ļ�
            {
                animator.SetBool("isAttack", true);
                Attack1();
                animator.SetInteger("AttackIndex", 0);
            }
            else if (AttackIndex == 2)  //��
            {
                animator.SetBool("isAttack", true);
                Attack2();
                animator.SetInteger("AttackIndex", 0);
            }
            else if (AttackIndex == 3)  //����Ļ�
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
                //��ȡ�����
                if (child.name == "LeftWing")
                {
                    leftWing = child.gameObject;
                }
                if (child.name == "RightWing")
                {
                    rightWing = child.gameObject;
                }
                //��ת���(����ʱ����z�Ḻ����ת,�ҳ��z��������ת)
                float minZRotation = -45;
                float maxZRotation = 45;
                float preRotation = leftWing.transform.rotation.z;
                bool rotateDir = true;  //����Ķ�����,true����
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
                    if (child.name == "������������" && collision.gameObject.name == "Player" && child.gameObject == gameObject)
                    {
                        SendMessage("�˴�������ܻ��ĺ���,ִ������ֵ����", Atk1_harm);
                    }
                }
            }
            else if(AttackIndex == 2)
            {
                if(collision.gameObject.name == "Player")
                {
                    SendMessage("�˴�������ܻ��ĺ���,ִ������ֵ����", Atk2_harm);
                }
            }
            else if (AttackIndex == 3)
            {
                if (collision.gameObject.name == "Player")
                {
                    SendMessage("�˴�������ܻ��ĺ���,ִ������ֵ����", Atk3_harm);
                }
            }
        }
    }
}