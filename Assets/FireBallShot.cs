using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallShot : MonoBehaviour
{
    public GameObject fireball;
    public GameObject fireballSpawnPoint;
    GameObject player;
   
    float timePassed = 0f;
    public GameObject explosion;
    GameObject fireball2;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        
        timePassed += Time.deltaTime;
        if (timePassed > 16.5f)
        {
            attack();
            timePassed = 0f;
        }
    }
        private void attack()
        {
            fireball2 = Instantiate(fireball, fireballSpawnPoint.transform.position, Quaternion.identity);
            fireball2.transform.parent = null;
            fireball2.GetComponent<Rigidbody>().AddForce(transform.forward * 7,
                                                        ForceMode.VelocityChange);
            Destroy(fireball2, 15);
        }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(explosion, fireball2.transform.position, Quaternion.identity);
            Destroy(fireball2.gameObject);

        }
    }
}


