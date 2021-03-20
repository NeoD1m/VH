using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimatorController
{
    private Animator animator;
    private Transform transform;
    public AIAnimatorController(Animator a, Transform t)
    {
        animator = a;
        transform = t;
    }

    public void stopRun()
    {
        animator.SetBool("run", false);
    }

    public void Run(Vector2 direction)
    {
        animator.SetBool("run", true);
        if (direction.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
