using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem.Processors;
using Random = UnityEngine.Random;
using UnityEngine.Windows;
using Unity.Netcode;
using Input = UnityEngine.Input;
using Unity.Collections;
using Cinemachine;
using Unity.VisualScripting;
using Mono.Cecil;

public class PlayerController : NetworkBehaviour
{

    public float sightRange = 10f;



    Vector2 inputVector;
    Rigidbody rb;
    Transform bulletSpawn;
    Transform bullet;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform zombiePrefab;

    zombieBehavior zombie;

    public float bulletSpeed = 20f;
    public float playerSpeed = 1.5f;
    public float hp = 10;
    public GameObject hpBar;

    public float rotationSpeed = 1.5f;

    [SerializeField] private GameObject kamera;
    //animacje
    Animator animator;
    string currentState;
    const string PLAYER_SHOOT_IDLE = "m_pistol_idle_A";
    const string PLAYER_SHOOT_RUN = "m_pistol_run";
    const string PLAYER_DEATH = "m_death_A";
     Vector3 lastPos;
    bool isDead;

    public AudioClip zombieGrowl; 
    public AudioSource source;
    public AudioClip clip;


    public float shootRange = 15f;
    public AudioClip clip2;
    float timePassed = 0f;

    PlayerInput PlInput;

    
    Scrollbar hpScrollBar;
    Vector2 movementVector;
    GameObject levelcontroller;
    // Start is called before the first frame update






    void Start()
    {


        if (IsClient && IsOwner)
        {
            Instantiate(kamera);

        }
        PlInput = GetComponent<PlayerInput>();
        isDead = false;
        movementVector = Vector2.zero;
        rb = GetComponent<Rigidbody>();
        inputVector = Vector2.zero;
        bulletSpawn = transform.Find("bulletSpawn");
        hpScrollBar = hpBar.GetComponent<Scrollbar>();
        animator = GetComponent<Animator>();


        levelcontroller = GameObject.Find("LevelController");
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;




        timePassed += Time.deltaTime;
        if (timePassed > Random.Range(7, 15))
        {
            
            source.PlayOneShot(zombieGrowl, 0.2f);
            timePassed = 0f;
        }



        transform.Rotate(Vector3.up * movementVector.x * rotationSpeed);
       
        transform.Translate(Vector3.forward * movementVector.y * Time.deltaTime * playerSpeed);

        //Sprawdza i zmienia animacje z biegania na idle
        //Próbowałem z tą funkcją OnMove, ale coś nie działa. 
        var moving = lastPos != transform.position;
        if (isDead == false)
        {
            if (moving && Math.Abs(lastPos.x - transform.position.x) > 0.001 || moving && Math.Abs(lastPos.z - transform.position.z) > 0.001) 
            {
                ChangeAnimationState(PLAYER_SHOOT_RUN);
            }
            else
            {
                ChangeAnimationState(PLAYER_SHOOT_IDLE);
            }
            lastPos = transform.position;
        }
        


    }

    void OnMove(InputValue inputValue)
    {
        if (!IsOwner) return;

        movementVector = inputValue.Get<Vector2>();
        //Debug.Log(movementVector.ToString());
    }


        void OnFire()
    {

        if (!IsOwner) return;

        Vector3 raySource = bulletSpawn.transform.position;
        Vector3 rayDirection = bulletSpawn.transform.forward;
        Debug.DrawRay(raySource, rayDirection);
        bool hasDone = false;
        ShootServerRPC(raySource, rayDirection, hasDone);
            




                /*
                bullet = Instantiate(bulletPrefab, bulletSpawn);
                bullet.parent = null;
                bullet.GetComponent<NetworkObject>().Spawn(true);


                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward*bulletSpeed,ForceMode.VelocityChange );
                Destroy(bullet, 1.5f);
                */
                source.PlayOneShot(clip2, 0.1f);
        
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;
        if(collision.gameObject.CompareTag("Enemy"))
        {
            //Vector3 pushVector = collision.gameObject.transform.position;
            //collision.gameObject.GetComponent<Rigidbody>().AddForce(pushVector.normalized, ForceMode.Impulse);
            Invoke("minusHp", 0.4f);


            if (hp <= 0)
            {
                PlInput.enabled = false;
                hpBar.active = false;
                hpScrollBar.enabled = false;
                isDead = true;
                ChangeAnimationState(PLAYER_DEATH);
                Invoke("GameOver", 2);
            }
            

        }
        if (collision.gameObject.CompareTag("medicalKit"))
        {
            Destroy(collision.gameObject);
            hp = 10;
            hpScrollBar.size = 1;
        }
            if (collision.gameObject.CompareTag("fireBall"))
        {
            isDead = true;
            ChangeAnimationState(PLAYER_DEATH);
            Invoke("GameOver", 2);

        }
        if (collision.gameObject.CompareTag("heal"))
        {
            hp = 10;
            hpScrollBar.size = hp / 10;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("fireBall"))
        {
            PlInput.enabled = false;
            hpBar.active = false;
            hpScrollBar.enabled = false;
            isDead = true;
            Destroy(collision.gameObject, 1);
            hp = 0;
            hpScrollBar.size = 0;

        }
    }
    private void GameOver()
    {
        GameMenager.Dead();
    }
    private void minusHp()
    {
        hp--;
        source.PlayOneShot(clip, 0.5f);
        hpScrollBar.size = hpScrollBar.size - 0.1f;

    }
    private void ChangeAnimationState(string newState)
    {
        if(newState == currentState)
        {
            return;
        }
        animator.Play(newState);
        currentState = newState;
    }

    bool isAnimationPlaying(Animator animator, string stateName)
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
            return true;
        }
        else
        {
            return false;
        }
    }



    [ServerRpc(RequireOwnership = false)]
    private void ShootServerRPC(Vector3 position, Vector3 direction, bool hasDone)
    {
        if (hasDone == false)
        { 
            if (Physics.Raycast(position, direction, out RaycastHit hit, shootRange))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                

                    zombieBehavior.hp--;
                    hasDone = true;
                }
                
                

                
            }

        }
    }




}
