using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStartSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataService a = new DataService("data.db"); //DB INIT
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
