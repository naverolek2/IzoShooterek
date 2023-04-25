using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

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

    public AudioSource source;
    public AudioClip clip;
    
    public AudioClip clip2;



    Scrollbar hpScrollBar;
    Vector2 movementVector;
    GameObject levelcontroller;
    // Start is called before the first frame update
    void Start()
    {
        movementVector = Vector2.zero;
        rb = GetComponent<Rigidbody>();
        inputVector = Vector2.zero;
        bulletSpawn = transform.Find("bulletSpawn");
        hpScrollBar = hpBar.GetComponent<Scrollbar>();
        

        levelcontroller = GameObject.Find("LevelController");
    }

    // Update is called once per frame
    void Update()
    {
       
        transform.Rotate(Vector3.up * movementVector.x);
       
        transform.Translate(Vector3.forward * movementVector.y * Time.deltaTime * playerSpeed);
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
            hp--;
            hpScrollBar.size = hpScrollBar.size - 0.1f;
            //Vector3 pushVector = collision.gameObject.transform.position;
            //collision.gameObject.GetComponent<Rigidbody>().AddForce(pushVector.normalized, ForceMode.Impulse);
            


            if (hp <= 0)
            {
                GameMenager.Dead();
            }
            

        }
        if (collision.gameObject.CompareTag("heal"))
        {
            hp = 10;
            hpScrollBar.size = hp / 10;
            Destroy(collision.gameObject);
        }
    }
   
    

}
