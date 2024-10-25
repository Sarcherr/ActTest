using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class AttackRange : MonoBehaviour
{
    Unit.Player parent;
    List<GameObject> targets = new List<GameObject>();

    private void Start()
    {
        parent = transform.parent.gameObject.GetComponent<Unit.Player>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && !targets.Contains(collision.gameObject))
        {
            targets.Add(collision.gameObject);
            //对目标造成伤害

        }
    }

    private void OnDisable()
    {


        targets.Clear();
    }

    /// <summary>
    /// 根据当前攻击类型造成伤害
    /// </summary>
    public void MakeDamage()
    {
        switch(parent.fsm.CurrentStateKind)
        {
            case StateKind.Attack_normal:
                break;


        }
    }
}
