using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMeAfter : MonoBehaviour
{
    public GameObject obj;
    public float frameLimit;
    int frame=0;
    void Update()
    {
        frame++;
        if (frame >= frameLimit) Destroy(obj);
    }
}
