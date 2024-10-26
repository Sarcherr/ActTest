using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class AttackRange : MonoBehaviour
{
    Unit.Player parent;
    List<GameObject> targets = new List<GameObject>();
    StateKind state = StateKind.Default;
    bool hasHit = false;
    //Boss boss = null;
    //Baby baby = null;

    private void Start()
    {
        parent = transform.parent.gameObject.GetComponent<Unit.Player>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        state = parent.fsm.CurrentStateKind;
        if (collision.CompareTag("Enemy") && !targets.Contains(collision.gameObject))
        {
            hasHit = true;
            Debug.Log("打中了喵");
            //boss = gameObject.GetComponent<Boss>();
            //baby = gameObject.GetComponent<Baby>();
            targets.Add(collision.gameObject);
            //对目标造成伤害
            if(true/*boss != null*/)
            {
                switch(state)
                {
                    case StateKind.Attack_normal:
                        //boss.BeHurt(parent.attackDamage_normal, false);
                        break;

                    case StateKind.Attack_sky:
                        //boss.BeHurt(parent.attackDamage_normal, false);
                        break;

                    case StateKind.Attack_heavy:
                        //boss.BeHurt(parent.attackDamage_normal, true);
                        break;

                    case StateKind.Attack_skill_1:
                        //boss.BeSkillHurt(parent.attackDamage_skill_1);
                        break;

                    case StateKind.Attack_skill_2:
                        switch(parent.skillNow)
                        {
                            case SkillHasUse.skill_2_1:
                                //boss.BeSkillHurt(parent.attackDamage_skill_2_1);
                                break;

                            case SkillHasUse.skill_2_2:
                                //boss.BeSkillHurt(parent.attackDamage_skill_2_2);
                                break;

                            case SkillHasUse.skill_2_3:
                                //boss.BeSkillHurt(parent.attackDamage_skill_2_3);
                                break;
                        }
                        break;
                }
            }
            else
            {
                switch (state)
                {
                    case StateKind.Attack_normal:
                        //baby.BeHurt(parent.attackDamage_normal, false);
                        break;

                    case StateKind.Attack_sky:
                        //baby.BeHurt(parent.attackDamage_normal, false);
                        break;

                    case StateKind.Attack_heavy:
                        //baby.BeHurt(parent.attackDamage_normal, true);
                        break;

                    case StateKind.Attack_skill_1:
                        //baby.BeSkillHurt(parent.attackDamage_skill_1);
                        break;

                    case StateKind.Attack_skill_2:
                        switch (parent.skillNow)
                        {
                            case SkillHasUse.skill_2_1:
                                //baby.BeSkillHurt(parent.attackDamage_skill_2_1);
                                break;

                            case SkillHasUse.skill_2_2:
                                //baby.BeSkillHurt(parent.attackDamage_skill_2_2);
                                break;

                            case SkillHasUse.skill_2_3:
                                //baby.BeSkillHurt(parent.attackDamage_skill_2_3);
                                break;
                        }
                        break;
                }
            }

            //boss = null;
            //baby = null;
        }
    }

    private void OnDisable()
    {
        if(hasHit)
        {
            switch(state)
            {
                case StateKind.Attack_normal:
                    parent.SPChange(parent.attackGain_normal);
                    break;

                case StateKind.Attack_sky:
                    parent.SPChange(parent.attackGain_sky);
                    break;

                case StateKind.Attack_heavy:
                    parent.SPChange(parent.attackGain_heavy);
                    break;
            }
        }
        targets.Clear();
    }
}
