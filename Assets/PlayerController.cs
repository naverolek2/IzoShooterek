﻿using System.Collections;
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

public class PlayerController : NetworkBehaviour
{
    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            _int = 56,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where  T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    };


    Vector2 inputVector;
    Rigidbody rb;
    Transform bulletSpawn;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float playerSpeed = 1.5f;
    public float hp = 10;
    public GameObject hpBar;

    public float rotationSpeed = 1.5f;


    //animacje
    Animator animator;
    string currentState;
    const string PLAYER_IDLE = "m_idle_A";
    const string PLAYER_SHOOT_IDLE = "m_pistol_idle_A";
    const string PLAYER_RUN = "m_run";
    const string PLAYER_SHOOT_RUN = "m_pistol_run";
    const string PLAYER_DEATH = "m_death_A";
     Vector3 lastPos;
    bool isDead;

    public AudioClip zombieGrowl; 
    public AudioSource source;
    public AudioClip clip;
    
    public AudioClip clip2;
    float timePassed = 0f;
    PlayerInput PlInput;


    Scrollbar hpScrollBar;
    Vector2 movementVector;
    GameObject levelcontroller;
    // Start is called before the first frame update
    private void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        };
    }


    void Start()
    {
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

        if(Input.GetKeyDown(KeyCode.T))
        {
            randomNumber.Value = new MyCustomData
            {
                _int = 10,
                _bool = false,
                message = "You're gay",
            };
        }



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
        movementVector = inputValue.Get<Vector2>();
        //Debug.Log(movementVector.ToString());
    }

    void OnFire()
    {
        
        
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn);
        bullet.transform.parent = null;
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward*bulletSpeed,ForceMode.VelocityChange );
        Destroy(bullet, 1);
        
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
   
    

}
