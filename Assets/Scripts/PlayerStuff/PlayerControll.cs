using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//TODO
/*
 * 1. ГГ дергается при диагАНАЛЬНОМ перемещении
 * 
 * 
 * 
 * 
 * 
 */
public class PlayerControll : MonoBehaviour
{
    [Header("AI stuff")]
    public TotalCommander totalCommander;

    [Header("Other stuff")]
    // PLayer
    public GameObject Player;
    private Rigidbody2D playerRigitBody;
    public GameObject armPivot;
    public int arrowDamage, meleeDamage;
    public PlayerAnimationChooser playerAnimationChooser;
    // Roll
    const int startRollFrame = 12;
    const int endRollFrame = 15;
    const int startImmortalityFrame = 6;
    const int endImmortalityFrame = 14;
    const int endRecoveryFrame =25;
    int rollMultiplayer = 2;
    public GameObject damgageCollider;


    // Movement
    public float speed;

    //stamina
    public float stamina,staminaForRoll,staminaForMelee;
    private float staminaTimer = 100;
    private float staminaTimer2, maxStamina;
    public Slider staminaSlider;

    //Gun & Knife
    public Camera cam;
    public Rigidbody2D gunRigitBody;
    public Rigidbody2D actualGunRigitBody;
    Vector2 mousePos;
    public GameObject arrow;
    public Slider rechargeSlider;
    //public GameObject Gun;
    public GameObject arrowSpawnPoint;
    public GameObject Knife;
    SpriteRenderer gunSpriteRenderer;
    GameObject arrowInstans;

    int hitFrame = 0;
    const int startHitFrame = 1;
    const int endHitFrame = 70;
    const int endHitRecoveryFrame = 100;
    bool hitRecoveryBool = false;

    int shotFrame = 0;
    const int startShotFrame = 1;
    const int endShotRecoveryFrame = 100;
    bool shootRecoveryBool = false;

    // Do not change
    private int rollMultiplayerStock = 1;
    public int rollFrame = 0;
    private bool rollRecoveryBool = false;

    void Start()
    {
        playerRigitBody = Player.GetComponent<Rigidbody2D>();
        staminaInit();
        rechargeSliderInit();
    }

    void Update()
    {
        Move();
        Roll();
        Health();
        Shoot();
        Hit();
        staminaSlider.value = stamina;
        if (staminaTimer == 0 && stamina < maxStamina) stamina += 0.2f;
        if (staminaTimer > 0) staminaTimer -= 1;
    }

    void rechargeSliderInit()
    {
        rechargeSlider.maxValue = endShotRecoveryFrame;
        rechargeSlider.minValue = 0;
        rechargeSlider.value = 0;
        rechargeSlider.gameObject.SetActive(false);
    }
    void staminaInit()
    {
        //staminaTimer = 150;
        maxStamina = stamina;
        staminaTimer2 = staminaTimer;
        staminaSlider.maxValue = stamina;
        staminaSlider.value = stamina;
        staminaSlider.minValue = 0;
    }

    public int HP;
    public Image[] hearts;
    public Sprite heartON, heartOFF;
    private int lastFrameHP;

    void Health()
    {
        if (lastFrameHP != HP)
        {
            lastFrameHP = HP;
            for (int i = 0; i < 3; i++)
            {
                if (HP-1 >= i) hearts[i].sprite = heartON; else hearts[i].sprite = heartOFF;
            }
        }
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal * speed * rollMultiplayerStock, moveVertical * speed * rollMultiplayerStock);
        playerRigitBody.velocity = movement;
    }

    void Shoot()
    {
        if (Input.GetKeyDown("mouse 0") && !shootRecoveryBool)
        {
            rechargeSlider.gameObject.SetActive(true);
            shootRecoveryBool = true;
            shot();
        }
        if (shootRecoveryBool)
        {
            if (shotFrame < endShotRecoveryFrame)
            {

                rechargeSlider.value = shotFrame; 
                shotFrame++;
            }
            if (shotFrame == endShotRecoveryFrame) { rechargeSlider.gameObject.SetActive(false); shotFrame = 0; shootRecoveryBool = false; }
        }
    }
    void shot()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - playerRigitBody.position;
        arrowInstans = Instantiate(arrow, arrowSpawnPoint.transform.position, Quaternion.LookRotation(lookDir.normalized, Vector2.up) * Quaternion.Euler(0, -90f, -90f));
        Rigidbody2D arrowInstansRigitBody = arrowInstans.GetComponent<Rigidbody2D>();
        arrowInstansRigitBody.AddForce(lookDir.normalized * 1000);
        //mousePos
        totalCommander.fakeBullets.Add(arrowInstans.GetComponent<ArrowToDodge>().fakeCollider); // Послать в TotalCommander коллайдер стрелы, чтобы Вампир (или други объекты) могли заранее предсказать попадание стрелы.
        totalCommander.realBullets.Add(arrowInstans.GetComponent<Collider2D>()); // Послать в TotalCommander коллайдер стрелы, чтобы Вампир (или другие объекты) получали урон.
    }

    void Hit()
    {
        if (Input.GetKeyDown("mouse 1") && stamina >= staminaForMelee && hitRecoveryBool == false)
        {
            hitRecoveryBool = true;
            stamina -= staminaForMelee;
            staminaTimer = staminaTimer2;
        }
        if (hitRecoveryBool)
        {
            hitFrame++;
            switch (hitFrame)
            {
                case (startHitFrame): Knife.SetActive(true);  break;
                case (endHitFrame):  Knife.SetActive(false); break;
                case (endHitRecoveryFrame): hitRecoveryBool = false; hitFrame = 0; break;
            }
        }
    }

    void Roll()
    {
        if (Input.GetKeyDown("left shift") && rollRecoveryBool == false && stamina >= staminaForRoll)
        {
            rollRecoveryBool = true;
            stamina -= staminaForRoll;
            staminaTimer = staminaTimer2;
        }
        if (rollRecoveryBool == true)
        {
            rollFrame++;
            switch (rollFrame)
            {
                case startRollFrame: rollMultiplayerStock = rollMultiplayer;break;
                case endRollFrame: rollMultiplayerStock = 1; break;
                case startImmortalityFrame: damgageCollider.SetActive(false); break;
                case endImmortalityFrame: damgageCollider.SetActive(true); break;
                case endRecoveryFrame: rollRecoveryBool = false; rollFrame = 0; break;
            }
        }
    }

    public bool isRofling()
    {
        return rollFrame > 0;
    }

    public bool isNotHit()
    {
        return hitFrame < startHitFrame;
    }
}