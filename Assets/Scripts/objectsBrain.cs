using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectsBrain : MonoBehaviour
{
    [Tooltip("if HP = 0, object will die from 1 hit")]
    public int HP;
    [Header("Optional settings")]
    public bool indestructible;
    public GameObject debree,debreeSpawnPoint;
    private int arrowDamage,knifeDamage;
    private void Start()
    {
        arrowDamage = FindObjectOfType<PlayerControll>().arrowDamage;
        knifeDamage = FindObjectOfType<PlayerControll>().meleeDamage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "arrow" || collision.tag == "enemy projectile")
        {
            if (!indestructible)
            {
                HP -= arrowDamage;
                if (HP > 0)
                {
                    Destroy(collision.gameObject);
                }
                else
                {
                    Debree();
                    Destroy(collision.gameObject);
                    Destroy(transform.gameObject);

                }
            } else Destroy(collision.gameObject);
        }
        if (collision.tag == "knife")
        {
            if (!indestructible)
            {
                HP -= knifeDamage;
                if (HP <= 0)
                {
                    Debree();
                    Destroy(transform.gameObject);
                }
            }
        }
    }
    void Debree()
    {
        if (debree != null)
        {
            if (debreeSpawnPoint != null)
                GameObject.Instantiate(debree, debreeSpawnPoint.transform.position, debreeSpawnPoint.transform.rotation);
            else
                GameObject.Instantiate(debree, transform.position, transform.rotation);
        }
    }
}
