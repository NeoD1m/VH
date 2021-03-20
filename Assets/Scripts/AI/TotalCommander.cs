using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalCommander : MonoBehaviour
{
    // Класс создан по рекомендациям из статьи на Хабре
    // "Чем быстрее вы забудете ООП, тем лучше для вас и ваших проектов"

    public GameObject player,boss;
    public Collider2D playerCollider, bossCollider;
    public List<Collider2D> fakeBullets;
    public List<Collider2D> realBullets;
    public List<Collider2D> obstacles;

    // Update is called once per frame
    void Update()
    {
        // clear bullet lists
        fakeBullets.RemoveAll(isNUll);
        realBullets.RemoveAll(isNUll);
        obstacles.RemoveAll(isNUll);
    }
    
    System.Predicate<object> isNUll = delegate (object x) { return x.ToString() == "null"; }; // Predicate can delete ANY NULL objects in ANY array.
}
