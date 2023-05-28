using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public GameObject zombie;
    public GameObject[] spawnpoint;
    

    void Start()
    {
        for (int i = 0; i < spawnpoint.Length; i++)
        {
            Instantiate(zombie, spawnpoint[i].transform.position, Quaternion.identity);
            

        }

    }

    // Update is called once per frame
    void Update()
    {
    }
    

    }
