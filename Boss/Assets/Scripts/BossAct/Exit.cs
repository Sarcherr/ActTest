using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    //Boss受伤和硬直时停止其他脚本
    Animator animator;
    MonoBehaviour[] otherScripts;
    BossAct.HurtAndPause H_and_P;

    void Start()
    {
        animator = GetComponent<Animator>();
        otherScripts = GetComponents<MonoBehaviour>();
    }
    void Update()
    {
        if (H_and_P.animName == "hurtAnim" || H_and_P.animName == "skillHurt")
        {
            OnEnterHurtandPause(animator);
        }
        else if (H_and_P.animName == "PauseShortAnim" || H_and_P.animName == "PauseLongAnim")
        {
            OnEnterHurtandPause(animator);
        }
        else if (H_and_P.animName == "红温Anim" || H_and_P.animName == "DeadAnim")
        {
            OnEnterHurtandPause(animator);
        }
    }
    public void OnEnterHurtandPause(Animator animator)
    {
        animator.Play(H_and_P.animName);
        foreach (var script in otherScripts)
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }
    }
    public void OnAnimationEnded(Animator animator)
    {
        foreach (var script in otherScripts)
        {
            if (script != this)
            {
                script.enabled = true;
            }
        }
    }
}
