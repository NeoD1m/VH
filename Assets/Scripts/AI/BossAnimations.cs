using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimations : MonoBehaviour
{
    public Animator bossAnimator;

    void Start()
    {
        
    }

    void Update()
    {
        bossAnimator.SetBool("attack", false);
    }
    public void attackAnim()
    {
        bossAnimator.SetBool("attack", true);
    }
}
