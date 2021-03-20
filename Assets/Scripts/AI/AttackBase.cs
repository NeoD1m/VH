using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    // BASED

    // Types of attacks
    public const int TYPE_BULLET = 228;
    public const int TYPE_MELEE = 1488;

    // Types of ranges
    public const int RANGE_MELEE = 0;       // Best for melee attacks
    public const int RANGE_SHORT = 1;       // Best for long melee attacks, rush and shotgun-like
    public const int RANGE_MID = 2;         // Best for rifle-like attacks 
    public const int RANGE_LONG = 3;        // Best for traps (https://goo-gl.ru/rDjp7) placing, sniper-like and non-target magic attacks
    public const int RANGE_EXTRA_LONG = 4;  // Best for healing, self-buffing, non-target magic attacks

    /*
    ⣿⣿⣿⣻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
    ⣿⣿⣿⣵⣿⣿⣿⠿⡟⣛⣧⣿⣯⣿⣝⡻⢿⣿⣿⣿⣿
    ⣿⣿⣿⣿⣿⠋⠁⣴⣶⣿⣿⣿⣿⣿⣿⣿⣦⣍⢿⣿⣿
    ⣿⣿⣿⣿⢷⠄⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣏⢼⣿
    ⢹⣿⣿⢻⠎⠔⣛⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡏⣿
    ⢸⣿⣿⠇⡶⠄⣿⣿⠿⠟⡛⠛⠻⣿⡿⠿⠿⣿⣗⢣⣿
    ⠐⣿⣿⡿⣷⣾⣿⣿⣿⣾⣶⣶⣶⣿⣁⣔⣤⣀⣼⢲⣿
    ⠄⣿⣿⣿⣿⣾⣟⣿⣿⣿⣿⣿⣿⣿⡿⣿⣿⣿⢟⣾⣿
    ⠄⣟⣿⣿⣿⡷⣿⣿⣿⣿⣿⣮⣽⠛⢻⣽⣿⡇⣾⣿⣿
    ⠄⢻⣿⣿⣿⡷⠻⢻⡻⣯⣝⢿⣟⣛⣛⣛⠝⢻⣿⣿⣿
    ⠄⠸⣿⣿⡟⣹⣦⠄⠋⠻⢿⣶⣶⣶⡾⠃⡂⢾⣿⣿⣿
    ⠄⠄⠟⠋⠄⢻⣿⣧⣲⡀⡀⠄⠉⠱⣠⣾⡇⠄⠉⠛⢿
    ⠄⠄⠄⠄⠄⠈⣿⣿⣿⣷⣿⣿⢾⣾⣿⣿⣇⠄⠄⠄⠄
    ⠄⠄⠄⠄⠄⠄⠸⣿⣿⠟⠃⠄⠄⢈⣻⣿⣿⠄⠄⠄⠄
    ⠄⠄⠄⠄⠄⠄⠄⢿⣿⣾⣷⡄⠄⢾⣿⣿⣿⡄⠄⠄⠄
    ⠄⠄⠄⠄⠄⠄⠄⠸⣿⣿⣿⠃⠄⠈⢿⣿⣿⠄⠄⠄ 
    */

    // Params
    public float cooldown;
    public string animationName;

    // Abstract functions
    public abstract int[] getDesirableRange();
    protected abstract int attack();
    public abstract int getType();

    // Functionality
    public void startAttack()
    {
        startAnim();
        attack();
    }

    private Animator animatorBoss = null;
    private void startAnim()
    {
        if (animatorBoss != null)
            animatorBoss.Play(animationName);
        else
            Debug.Log("ERROR!!!! Animator is NULL. It should be set in AttackChooser From Brain.");
    }

    public void setAnimator(Animator a)
    {
        animatorBoss = a;
    }

    public bool isAnimPlaying()
    {
        return animatorBoss.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
}
