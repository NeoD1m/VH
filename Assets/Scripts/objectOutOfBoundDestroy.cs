using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectOutOfBoundDestroy : MonoBehaviour
{
    public GameObject arrow;
    int frame=0;
    void Update()
    {
        frame++;
        if ((Mathf.Abs(transform.position.x) >= 100f) || (Mathf.Abs(transform.position.y) >= 100f)) Destroy(arrow);
        if (frame >= 500) Destroy(arrow);
    }
}
