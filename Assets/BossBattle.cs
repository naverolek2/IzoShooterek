using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 punktWywolwaczy;
    float odlegloscAktywacji = 5;
    bool czyAktywowany;
    public AudioSource source;
    public AudioClip clip;

    void Start()
    {
        punktWywolwaczy = GameObject.FindGameObjectWithTag("Your_Tag_Here").transform.position;
        czyAktywowany = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(czyAktywowany == false)
        {
        Collider[] heardObjects = Physics.OverlapSphere(punktWywolwaczy, odlegloscAktywacji);
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
        
    }
}
