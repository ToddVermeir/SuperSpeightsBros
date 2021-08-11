using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    

    public Vector3 spawnPosition;
    public Transform playerTransform;
    private int hitpoint = 3;

    private void Update()
    {
        if(playerTransform.position.y < -15)
        {
            playerTransform.position = spawnPosition;
        }
    }

}
