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
    int hp = 10;

    GameObject player;
    NavMeshAgent agent;
    private bool playerVisible = false;
    private bool playerHearable = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 raySource = transform.position + Vector3.up * 1f;
        Vector3 rayDest = player.transform.position - transform.position + Vector3.up * 1f;
        Vector3 rayDirection = player.transform.position - transform.position;
        RaycastHit hit;
        Debug.DrawRay(raySource, rayDirection);
        
        
            if (Physics.Raycast(raySource, rayDirection, out hit, sightRange))
            {
                Debug.Log(hit.transform.gameObject.name.ToString());

               

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

            //transform.Translate(Vector3.forward * Time.deltaTime);

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
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<NavMeshAgent>().enabled = false;
                transform.Translate(Vector3.up);
                transform.Rotate(Vector3.right * -90);
                Destroy(transform.gameObject, 1);
               


            }
        }

    }
   public Vector3 currentPosition() { 
    return transform.position;
    }
    


}
