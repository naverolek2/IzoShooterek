using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Processors;
using Random = UnityEngine.Random;

public class zombieBehavior : NetworkBehaviour
{
    public float sightRange = 15f;
    public float hearRange = 10f;
    public float attackRange = 5f;

    static public int hp = 4;
    float timePassed = 0f;
    float timePassed2 = 0f;
    GameObject[] player = new GameObject[8];
    NavMeshAgent agent;
    public GameObject medkit;

    Animator animator;
    string currentState;
    const string ZOMBIE_IDLE = "Z_idle_A";
    const string ZOMBIE_RUN = "Z_run";
    const string ZOMBIE_ATTACK = "atak_zombie";
    const string ZOMBIE_DEATH = "Z_death_A";
    
    


    private bool playerVisible = false;
    private bool playerHearable = false;
    int timeNeed;
    int timeNeed2;
    Rigidbody rb;
    
    Vector3 lastPos;
    bool hasAttacked;
    bool isDead;
    Collider collid;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timeNeed = Random.Range(7, 15);
        timeNeed2 = 2;
        collid = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        hasAttacked = false;
        agent.speed = 1;
        agent.angularSpeed = 120;
        isDead = false;



    }
    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            isDead = true;
            agent.speed = 0;
            agent.angularSpeed = 0;

            agent.isStopped = true;
            rb.detectCollisions = false;
            collid.enabled = false;
            agent.avoidancePriority = 0;

            ChangeAnimationState(ZOMBIE_DEATH);
            /*
            if (Random.Range(1, 8) == 3)
            {
                Instantiate(medkit, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
            }
            */
            Destroy(transform.gameObject, 2);



        }




        var moving = lastPos != transform.position;

        if (hasAttacked == false)
        {
            if (isDead == false)
            {
                if (moving && Math.Abs(lastPos.x - transform.position.x) > 0.001 || moving && Math.Abs(lastPos.z - transform.position.z) > 0.001)
                {
                    ChangeAnimationState(ZOMBIE_RUN);
                }
                else
                {
                    ChangeAnimationState(ZOMBIE_IDLE);
                }
                lastPos = transform.position;
            }
        }











        for (int i = 0; i < player.Length; i++)
        {
            Vector3 raySource = transform.position + Vector3.up * 1f;
            Vector3 rayDest = player[i].transform.position - transform.position + Vector3.up * 1f;
            Vector3 rayDirection = player[i].transform.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(raySource, rayDirection, out hit, sightRange))
            {
                //Debug.Log(hit.transform.gameObject.name.ToString());



                if (hit.transform.CompareTag("Player"))
                    playerVisible = true;
                else
                    playerVisible = false;


            }

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

            int ktory = 0;
            float dist = Vector3.Distance(player[0].transform.position, transform.position); ;
            for (int i = 0; i < player.Length; i++)
            {
                float dist2 = Vector3.Distance(player[i].transform.position, transform.position);
                if (dist2 < dist)
                {
                    dist = dist2;
                    ktory = i;
                }
            }
            agent.destination = player[ktory].transform.position;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            hp--;
            if (hp <= 0)
            {
                isDead = true;
                agent.speed = 0;
                agent.angularSpeed = 0;

                agent.isStopped = true;
                rb.detectCollisions = false;
                collid.enabled = false;
                agent.avoidancePriority = 0;

                ChangeAnimationState(ZOMBIE_DEATH);
                if(Random.Range(1, 8) == 3)
                {
                    Instantiate(medkit, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
                }
                Destroy(transform.gameObject, 2);
                


            }
        }
        */
        if(collision.gameObject.CompareTag("Player"))
        {
            if (hp > 0)
            {
                if(hasAttacked == false)
                {

                
                    //Debug.Log("Działa");
                    hasAttacked = true;
                    agent.speed = 0;
                    agent.angularSpeed = 0;

                    agent.isStopped = true;
                
                    ChangeAnimationState(ZOMBIE_ATTACK);
                
                    Invoke("actionResume", 1);
                }
            }
            else
            {
                isDead = true;
                agent.speed = 0;
                agent.angularSpeed = 0;

                agent.isStopped = true;
                rb.detectCollisions = false;
                ChangeAnimationState(ZOMBIE_DEATH);
                Destroy(transform.gameObject, 2);
                this.GetComponent<NetworkObject>().Despawn(true);

            }



        }



    }
    
    private void actionResume()
    {
        agent.isStopped = false;
        hasAttacked = false;
        agent.speed = 1;
        agent.angularSpeed = 120;
        

    }
    public Vector3 currentPosition() { 
    return transform.position;
    }
    

    private void ChangeAnimationState(string newState)
    {
        if (newState == currentState)
        {
            return;
        }
        animator.Play(newState);
        currentState = newState;
    }

    bool isAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
