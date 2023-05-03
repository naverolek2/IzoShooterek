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

public class PlayerController : MonoBehaviour
{
    Vector2 inputVector;
    Rigidbody rb;
    Transform bulletSpawn;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float playerSpeed = 1.5f;
    public float hp = 10;
    public GameObject hpBar;
    public Text ammo;
    int ammoAmount = 30;
    public int ammoAmountMax = 30;

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


    public AudioSource source;
    public AudioClip clip;
    
    public AudioClip clip2;



    Scrollbar hpScrollBar;
    Vector2 movementVector;
    GameObject levelcontroller;
    // Start is called before the first frame update
    void Start()
    {
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
       
        transform.Rotate(Vector3.up * movementVector.x);
       
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
        Destroy(bullet, 5  );
        source.PlayOneShot(clip2, 0.5f);
        
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;
        if(collision.gameObject.CompareTag("Enemy"))
        {
            source.PlayOneShot(clip);
            //Vector3 pushVector = collision.gameObject.transform.position;
            //collision.gameObject.GetComponent<Rigidbody>().AddForce(pushVector.normalized, ForceMode.Impulse);
            Invoke("minusHp", 0.4f);


            if (hp <= 0)
            {
                isDead = true;
                ChangeAnimationState(PLAYER_DEATH);
                Invoke("GameOver", 2);
            }
            

        }
        if (collision.gameObject.CompareTag("heal"))
        {
            hp = 10;
            hpScrollBar.size = hp / 10;
            Destroy(collision.gameObject);
        }
    }
    private void GameOver()
    {
        GameMenager.Dead();
    }
    private void minusHp()
    {
        hp--;
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
