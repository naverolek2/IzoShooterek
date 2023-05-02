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
    GameObject player;
    public GameObject healPrefab;

    int zombieCounter = 0;
  
   

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
            if(GameObject.FindGameObjectsWithTag("heal").Length < 3 )
            {
                Instantiate(healPrefab, randomPosition(), Quaternion.identity);
            }
        for (int i = 0; i < 3; i++)
        {
            if(table[i].IsDestroyed())
            {
                table[i] = null;
                zombieCounter--;
               
            }
            if (table[i] == null)
            {
                  //table[i] = Instantiate(zombie, randomPosition(), Quaternion.identity);
            }
            
          
        
            
        }
        
        
        
     



    }
    private Vector3 randomPosition()
    {
        Vector3 spawnPoint;
        do
        {
            int randomPosX = Random.Range(-52, 52);
            int randomPosY = Random.Range(-51, 51);

            spawnPoint = new Vector3(randomPosX, 4
                , randomPosY);
            spawnPoint = spawnPoint.normalized * Random.Range(12, 16);
            spawnPoint += player.transform.position;
        }
        while (Physics.CheckSphere(new Vector3(spawnPoint.x, 1, spawnPoint.z), 0.9f));      

       
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
