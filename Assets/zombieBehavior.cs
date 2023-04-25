using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class zombieBehavior : MonoBehaviour
{
    public float sightRange = 15f;
    public float hearRange = 10f;
    int hp = 4;
    float timePassed = 0f;
    float timePassed2 = 0f;
    GameObject player;
    NavMeshAgent agent;
    private bool playerVisible = false;
    private bool playerHearable = false;
    int timeNeed;
    int timeNeed2;
    Rigidbody rb;
    public AudioSource source;
    public AudioClip clip;
    




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        timeNeed = Random.Range(2, 5);
        timeNeed2 = 2;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        



    }
    // Update is called once per frame
    void Update()
    {
        timer();
        

        Vector3 raySource = transform.position + Vector3.up * 1f;
        Vector3 rayDest = player.transform.position - transform.position + Vector3.up * 1f;
        Vector3 rayDirection = player.transform.position - transform.position;
        RaycastHit hit;
        Debug.DrawRay(raySource, rayDirection);

        timePassed += Time.deltaTime;
        if (timePassed > timeNeed)
        {
            source.PlayOneShot(clip);
            timePassed = 0f;
        }

        if (Physics.Raycast(raySource, rayDirection, out hit, sightRange))
            {
                //Debug.Log(hit.transform.gameObject.name.ToString());

               

                if (hit.transform.CompareTag("Player"))
                    playerVisible = true;
                else
                    playerVisible = false;
                
                    
            }
        Collider[] heardObjects = Physics.OverlapSphere(transform.position, hearRange);

        playerHearable = false;
        foreach (Collider collider in heardObjects)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                playerHearable = true;
            }
        }


        agent.isStopped = !playerHearable && !playerVisible;
        
        if (hp > 0)
        {
            //transform.LookAt(player.transform.position);
            //Vector3 playerDirection = transform.position - player.transform.position;

            // transform.Translate(Vector3.back * Time.deltaTime);

            agent.destination = player.transform.position ;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            hp--;
            if (hp <= 0)
            {
               
                Destroy(transform.gameObject, 0);



            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            agent.speed = 0;
            agent.angularSpeed = 0;
            timePassed2 = 0;


        }


    }
   public Vector3 currentPosition() { 
    return transform.position;
    }
    void timer()
    {
        timePassed2 += Time.deltaTime;
        if (timePassed2 > timeNeed2)
        {
            agent.speed = 1;
            agent.angularSpeed = 120;
            timePassed2 = 0;
        }
    }


}
