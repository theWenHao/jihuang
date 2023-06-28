using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TreeController : BaseObject
{ 
    [SerializeField] Animator animator;

    private void Update()
    {
        
    }
    public override void Hurt(int damage)
    {
        base.Hurt(damage);
        animator.SetTrigger("Hurt");
        PlayAudio(0);
    }

    public override void Dead()
    {
        base.Dead();
        Destroy(gameObject);
    }
}
