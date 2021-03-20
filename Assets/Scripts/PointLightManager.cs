using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PointLightManager : MonoBehaviour
{
    public Light2D light1;
    
    public double minRange = 2;
    public double maxRange = 5;
    public int frequencyOfChangesMin = 7;
    public int frequencyOfChangesMax = 9;
    
    private int counter = 0;
    private int frequencyOfChanges = 7;
    private float step;
    System.Random rand = new System.Random();

    void NewRange()
    {
        frequencyOfChanges = rand.Next(frequencyOfChangesMin, frequencyOfChangesMax+1);
        step = (rand.Next((int)minRange*1000, (int)(maxRange+1)*1000)/1000- light1.pointLightOuterRadius) /frequencyOfChanges;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        light1.pointLightOuterRadius = (int) minRange;
    }

   
    // Update is called once per frame
    void Update()
    {
        if (minRange > maxRange)
        {
            maxRange = minRange;
        }
        if (frequencyOfChangesMax < frequencyOfChangesMin)
        {
            frequencyOfChangesMax = frequencyOfChangesMin;
        }
        if (counter >= frequencyOfChanges)
        {
            counter = 0;
            NewRange();
        }
        light1.pointLightOuterRadius += step;
        counter += 1;
        
    }
}
