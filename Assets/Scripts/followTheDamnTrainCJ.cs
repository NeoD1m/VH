using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTheDamnTrainCJ : MonoBehaviour
{
    public Transform playerPos,sliderPos;
    private Vector3 lastPos;
    void Update()
    {
        Vector3 playerPosVec = new Vector3(playerPos.position.x, playerPos.position.y + 1, playerPos.position.z);
        Vector3 newPos = Vector3.Lerp(playerPosVec, lastPos, 1);
        sliderPos.position = newPos;
        lastPos = playerPosVec;
    }
}
