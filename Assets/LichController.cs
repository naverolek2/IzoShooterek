using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class LichController : MonoBehaviour
{
    Scrollbar hpScrollBar;
    public GameObject hpBar;
    public TextMeshProUGUI procentHP;
    public int hp = 50;
    bool isDead;
    GameObject player;
    public GameObject fireball;
    public GameObject fireballSpawnPoint;
    Animator animator;
    string currentState;
    const string LICH_DEATH = "die";
    const string LICH_ATTACK = "attack01";
    const string LICH_IDLE = "idle";
    public float Force = 20;
    public GameObject zombie;
    float timePassed = 0f;
    float timePassed2 = 0f;
    public GameObject[] spawnpoints;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hpScrollBar = hpBar.GetComponent<Scrollbar>();
        isDead = false;
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform.position);
        timePassed += Time.deltaTime;
        if (timePassed > 5f)
        {
            attack();
            timePassed = 0f;
        }
        timePassed2 += Time.deltaTime;
        if (timePassed2 > 15f)
        {
            for (int i = 0; i < spawnpoints.Length; i++)
            {
                Instantiate(zombie, spawnpoints[i].transform.position, Quaternion.identity);
            }
            timePassed2 = 0f;
        }




    }

    private void attack()
    {
        GameObject fireball2 = Instantiate(fireball, fireballSpawnPoint.transform.position, Quaternion.identity);
        fireball2.transform.parent = null;
        fireball2.GetComponent<Rigidbody>().AddForce(transform.forward * 7,
                                                    ForceMode.VelocityChange);
        Destroy(fireball2, 15);
    }
    private void FixedUpdate()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            hpScrollBar.size = hpScrollBar.size - 0.02f;
            hp--;
            procentHP.text = hp + "hp";
            if (hp <= 0)
            {
                hpBar.active = false;
                hpScrollBar.enabled = false;
                procentHP.enabled = false;

                isDead = true;
                ChangeAnimationState(LICH_DEATH);
                Invoke("GameOver", 2);
            }
        }
    }
    private void GameOver()
    {
        GameMenager.Win();

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
