using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BossBattle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject punktWywolawczy;
    Vector3 punktWywolawczyVector;
    float odlegloscAktywacji = 7f;
    bool czyAktywowany;
    public AudioSource source;
    public AudioClip clip;
    public GameObject finalboss;
    public GameObject bossSpawnPoint;
    Vector3 bossSpawnPointVector;
    public GameObject zastawka;
    public GameObject zastawka2;
    public AudioClip clip2;
    public GameObject[] zastawkaSpawnPoint;


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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
     //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
     Gizmos.DrawWireSphere(punktWywolawczyVector, odlegloscAktywacji);
    }

    void akcjaStart()
    {
        SceneManager.LoadScene("cutscene");

        Vector3 bossSpawnPointVector = bossSpawnPoint.transform.position;
        Instantiate(finalboss, bossSpawnPointVector, Quaternion.identity);

        for(int i = 0; i < zastawkaSpawnPoint.Length -1; i++)
        {
            Instantiate(zastawka, zastawkaSpawnPoint[i].transform.position, Quaternion.identity);  
        }
        Instantiate(zastawka2, zastawkaSpawnPoint[zastawkaSpawnPoint.Length - 1].transform.position, Quaternion.identity);
    }
}
