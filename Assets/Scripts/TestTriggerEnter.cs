using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTriggerEnter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter Trigger");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enter");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
