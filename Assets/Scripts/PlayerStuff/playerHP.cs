using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHP : MonoBehaviour
{
    public PlayerControll playerControll;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemy projectile")
        {
            playerControll.HP--;
            Destroy(collision.gameObject);
        }
    }
}
