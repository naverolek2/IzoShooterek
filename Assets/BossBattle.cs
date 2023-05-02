using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject punktWywolawczy;
    Vector3 punktWywolawczyVector;
    float odlegloscAktywacji = 4.8f;
    bool czyAktywowany;
    public AudioSource source;
    public AudioClip clip;
    public GameObject finalboss;
    public GameObject bossSpawnPoint;
    Vector3 bossSpawnPointVector;
    
    public AudioClip clip2;


    void Start()
    {
        punktWywolawczyVector = punktWywolawczy.transform.position;
        czyAktywowany = false;

        // muzyka w tle
        source.clip = clip2;
        source.volume = 0.45f;
        source.Play();
        source.loop = true;
        // muzyka w tle
    }

    // Update is called once per frame
    void Update()
    {
        
        if(czyAktywowany == false)
        {
        Collider[] heardObjects = Physics.OverlapSphere(punktWywolawczyVector, odlegloscAktywacji);
        foreach (Collider collider in heardObjects)
            {
            if (collider.gameObject.CompareTag("Player"))
            {
                    czyAktywowany = true;
                    akcjaStart();
                
            }
        }
        }
        




    }
    void akcjaStart()
    {
        Vector3 bossSpawnPointVector = bossSpawnPoint.transform.position;
        Instantiate(finalboss, bossSpawnPointVector, Quaternion.identity);
    }
}
