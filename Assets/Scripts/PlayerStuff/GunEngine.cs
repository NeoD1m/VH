using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEngine : MonoBehaviour
{
    /**
     * Этот класс выбирает для игрока спрайт на основе положения курсора на экране и (возможно в будущем) других действий в игре
     * 
     * Теперь этот класс вертит пушку
     */
     private string LOG_TAG = "Gunengine > ";

    //[Header("Player sprites for all positions")]
    //public Sprite ggSector3;
    //public Sprite ggSector4;
    //public Sprite armSector3;
    //public Sprite armSector4;

    //[Header("Sprite Renderers")]
    //public SpriteRenderer playerSpriteRenderer;
    //public SpriteRenderer armSpriteRenderer;

    [Header("Rigid Bodies")]
    public Transform playerRigidBody;
    public Transform armPivot;
    public Transform gun;
    public float gunPadding;


    // Start is called before the first frame update
    private float defaultGunPosX = 0f;
    void Start()
    {
        defaultGunPosX = armPivot.localPosition.x;
        if (defaultGunPosX < 0)
            gunPadding = -gunPadding;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int mouseSector = getSector();
        chooseSprites(mouseSector);

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - armPivot.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x)*Mathf.Rad2Deg;
        armPivot.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }

    private void chooseSprites(int mouseSector)
    {
        if (mouseSector == 1 || mouseSector == 3)
        {
            gun.localRotation = Quaternion.Euler(0f, 0f, 0f);
            armPivot.localPosition = new Vector3(defaultGunPosX + gunPadding, armPivot.localPosition.y, armPivot.localPosition.z);
        }
        else
        {
            gun.localRotation = Quaternion.Euler(0f, 180f, 180f);
            armPivot.localPosition = new Vector3(defaultGunPosX + gunPadding, armPivot.localPosition.y, armPivot.localPosition.z);
        }
    }

    private int getSector()
    {
        // Get mouse pos
        //Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        // Middle coordinates
        //int height = 800 / 2;//Screen.currentResolution.height / 2;
        //int width = 610 / 2;//Screen.currentResolution.width / 2;
        //Debug.Log(LOG_TAG +height.ToString()+" "+width.ToString());
        //Debug.Log(LOG_TAG + screenPosition.x.ToString() + " " + screenPosition.y.ToString());

        //Debug.Log(LOG_TAG + screenPosition.x.ToString() + " " + screenPosition.y.ToString());

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerRigidBody.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (Mathf.Abs(rotationZ) > 90)
            return 2;
        else
            return 1;
    }
}
