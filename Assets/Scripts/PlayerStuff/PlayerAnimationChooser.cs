using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationChooser : MonoBehaviour
{
    public Animator anim;
    public Transform playerModel;
    public Transform armPivot;
    public PlayerControll playerControll;

    public Animator knifeAnim;
    private float lastAngle;
    //public float moveVertical;
    void Start()
    {
        
    }
    //https://youtu.be/vSTJk4_9bYk
    void noRoll()
    {
        anim.SetBool("roll", false);
        anim.SetBool("roll back", false);
    }
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        //Debug.Log("Горизонталь " + moveHorizontal);
        //Debug.Log("Угол " + armPivot.rotation.eulerAngles.z);

        float angle = armPivot.rotation.eulerAngles.z;
        if (playerControll.isRofling())
        {
            if (moveHorizontal >= 0 && (angle < 90 || angle > 270))
                anim.SetBool("roll", true);
            else

            if (moveHorizontal < 0 && (angle < 90 || angle > 270))
                anim.SetBool("roll back", true);
            else

            if (moveHorizontal <= 0 && (angle > 90 || angle < 270))
                anim.SetBool("roll", true);
            else

            if (moveHorizontal > 0 && (angle > 90 || angle < 270))
                anim.SetBool("roll back", true);
        } else noRoll();




        if (Mathf.Abs(moveVertical) + Mathf.Abs(moveHorizontal) > 0)
        {
            if (moveHorizontal > 0)
            {
                anim.SetBool("run", true);
                anim.SetBool("run back", false);
            }
            else
            {
                anim.SetBool("run", false);
                anim.SetBool("run back", true);
            }
        }
        else
        {
            anim.SetBool("run back", false);
            anim.SetBool("run", false);
        }

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerModel.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;


        if (Mathf.Abs(rotationZ) > 90)
        {
            if (lastAngle < 90)
            {
                playerModel.rotation = Quaternion.Euler(0, 180, 0);
                lastAngle = Mathf.Abs(rotationZ);
            }
        }
        else
        {
            playerModel.rotation = Quaternion.Euler(0, 0, 0);
            lastAngle = Mathf.Abs(rotationZ);
        }
    }

    public void setTrueKnifeAnim()
    {
        //knifeAnim.Play("melee");
        knifeAnim.SetBool("attack", true);
    }
    public void setFalseKnifeAnim()
    {
        //knifeAnim.Play("melee");
        knifeAnim.SetBool("attack", false);
    }
}
