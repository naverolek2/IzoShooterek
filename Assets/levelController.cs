using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class levelController : MonoBehaviour
{
    public GameObject zombie;
    GameObject[] table = new GameObject[3]; 
    
    int zombieCounter = 0;
    bool isZombieDead;
   

    // Start is called before the first frame update
    void Start()
    {
        
        Spawn();
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if(table[i].IsDestroyed())
            {
                table[i] = null;
                zombieCounter--;
            }
            if (table[i] == null)
            {
                  table[i] = Instantiate(zombie, randomPosition(), Quaternion.identity);
            }
            
            
        }
        
        
        {

        }
        



    }
    private Vector3 randomPosition()
    {
        int randomPosX = Random.Range(-52, 52);
        int randomPosY = Random.Range(-51,51);

        Vector3 spawnPoint = new Vector3(randomPosX, 5, randomPosY);
        spawnPoint = spawnPoint.normalized * Random.Range(12, 16);
        return spawnPoint;


}
void Spawn()
    {
        for(int i = 0; i < 3 ; i++)
        {
            table[i] = Instantiate(zombie, randomPosition(), Quaternion.identity);
            zombieCounter++;
        }
        
    }

}
