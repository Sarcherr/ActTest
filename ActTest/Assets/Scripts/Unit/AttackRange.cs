using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    List<GameObject> targets = new List<GameObject>();

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
}
