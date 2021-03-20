using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackChooser : MonoBehaviour
{
    [Header("Attack chooser stuff")]
    public AttackBase[] attacks;
    public Animator bossAnimator;
    public TotalCommander totalCommander;
    public Collider2D[] CollidersToIngoreWhenAiming;

    public GameObject DEBUGOBJECT;

    [Header("Distance convertation")]
    public float lengthMelee;
    public float lengthShort;
    public float lengthMid;
    public float lengthLong;


    private Vector2 direction;

    private float[] ratings;
    private float[] cooldowns;
    private List<AttackBase> topAttacks = new List<AttackBase>();
    private AttackBase currentAttack = null;

    private void Start()
    {
        ratings = new float[attacks.Length];
        cooldowns = new float[attacks.Length];
        
        for (int i = 0; i < attacks.Length; ++i)
        {
            ratings[i] = 0.5f;
            cooldowns[i] = attacks[i].cooldown;
            attacks[i].setAnimator(bossAnimator);
        }

        StartCoroutine(cooldownsUpdate());
    }

    public bool isNoObstaclesToPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(totalCommander.bossCollider.ClosestPoint(totalCommander.playerCollider.bounds.center), 
            totalCommander.playerCollider.ClosestPoint(totalCommander.bossCollider.bounds.center));

        //If something was hit, the RaycastHit2D.collider will not be null.
        if (hit.collider != null)
        {
            DEBUGOBJECT.transform.localPosition = hit.point;
            bool f = false;
            foreach(Collider2D collider in CollidersToIngoreWhenAiming)
            {
                if (hit.collider.name == collider.name)
                    f = true;
            }
            return f;
            //return false;
        }
        else
            return true;
    }

    IEnumerator cooldownsUpdate()
    {
        float updateRate = 0.5f;
        for (int i = 0; i < cooldowns.Length; ++i)
        {
            if (cooldowns[i] > 0)
                cooldowns[i] -= updateRate;
        }
        yield return new WaitForSeconds(updateRate);
        StartCoroutine(cooldownsUpdate());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DEBUGOBJECT.SetActive(!DEBUGOBJECT.activeSelf);
        }
    }

    public bool isAttacking()
    {
        if (currentAttack != null)
        {
            if (currentAttack.isAnimPlaying())
                return true;
            else
            {
                currentAttack = null;
                return false;
            }
        }
        else
            return false;
    }

    private bool isAttackCooldownZero(AttackBase attack)
    {
        int j = -1;
        for (int i = 0; i < attacks.Length; ++i)
        {
            if (attacks[i] == attack)
            {
                j = i;
                break;
            }
        }
        if (j >= 0)
            return cooldowns[j] <= 0;
        else
            return false;
    }

    public bool anyAttacksAvaliable()
    {
        return topAttacks.Count > 0;
    }

    public bool useBestAttack()
    {
        if (topAttacks.Count > 0)
        {
            if (ifBulletStartAttack(topAttacks[0]))
                return true;
            if (ifMeleeStartAttack(topAttacks[0]))
                return true;
        }
        return false;
    }

    private bool ifBulletStartAttack(AttackBase attack)
    {
        if (attack.getType() == AttackBase.TYPE_BULLET)
        {
            if (isNoObstaclesToPlayer())
            {
                ((AttackBullet)attack).direction = direction;
                ((AttackBullet)attack).setPositionToSpawn(transform.localPosition);
                attack.startAttack();

                //FindObjectOfType<BossAnimations>().attackAnim();//Анимация атаки
                // Автор строки выше: https://youtu.be/tjhuxBkivR8
                // @
                // Автору строки 10 лет
                // Блятб, АХАХАХХАХА, я посмотрел видео
                resetAttackCooldown(attack);
                currentAttack = attack;
                return true;
            }
        }
        return false;
    }

    private bool ifMeleeStartAttack(AttackBase attack)
    {
        if (attack.getType() == AttackBase.TYPE_MELEE)
        {
            //((AttackBullet)attack).direction = direction;
            //((AttackBullet)attack).setPositionToSpawn(transform.localPosition);
            ((AttackSpearMelee)attack).setDataForAttack(transform.localPosition, totalCommander.player.transform.localPosition);
            attack.startAttack();

            resetAttackCooldown(attack);
            currentAttack = attack;
            return true;
        }
        return false;
    }

    private void resetAttackCooldown(AttackBase attack)
    {
        for (int i = 0; i < attacks.Length; ++i)
            if (attack == attacks[i])
                cooldowns[i] = attack.cooldown;
    }

    public void setRatings(Vector2 toAim)
    {
        topAttacks.Clear();

        int range = convertVectorToAbstractRange(ShitUtilities.getVectorLength(toAim)); // Get range
        for (int i = 0; i < attacks.Length; ++i)
        {
            AttackBase attack = attacks[i];
            int[] attackRanges = attack.getDesirableRange();

            foreach (int r in attackRanges)
                if (r == range && isAttackCooldownZero(attack) && !topAttacks.Contains(attack))
                    topAttacks.Add(attack);
            
        }
        /*Debug.Log("-");
        Debug.Log(range);
        Debug.Log(topAttacks.Count);
        Debug.Log("-");*/
        // calc range diff
        // calc cooldown
    }

    public int convertVectorToAbstractRange(float length)
    {
        if (length < lengthMelee)
            return AttackBase.RANGE_MELEE;
        if (length < lengthShort)
            return AttackBase.RANGE_SHORT;
        if (length < lengthMid)
            return AttackBase.RANGE_MID;
        if (length < lengthLong)
            return AttackBase.RANGE_LONG;
        return AttackBase.RANGE_EXTRA_LONG;
    }

    public void doAnyAttack()
    {

        for (int i = 0; i < attacks.Length; ++i)
        {
            AttackBase attack = attacks[i];
            if (cooldowns[i] <= 0)
            {
                ifBulletStartAttack(attack);
            }
        }
    }

    public Vector2 getDirection()
    {
        return direction;
    }
    public void setDirection(Vector2 d)
    {
        direction = d.normalized;
    }
}
