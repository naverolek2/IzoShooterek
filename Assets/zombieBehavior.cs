using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class zombieBehavior : MonoBehaviour
{
    
    GameObject player;
    int hp = 6;
    public bool isDead = false;

    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        // Jeden strza³ odbiera 2 hp

    }
    // Update is called once per frame
    void Update()
    {
        if (hp > 0)
        {
            transform.LookAt(player.transform.position);
            //Vector3 playerDirection = transform.position - player.transform.position;

            transform.Translate(Vector3.forward * Time.deltaTime);
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
                isDead = true;
                transform.Translate(Vector3.up);
                transform.Rotate(Vector3.right * -90);
                GetComponent<BoxCollider>().enabled = false;
                Destroy(transform.gameObject);
                
                
            }
        }

    }
   public Vector3 currentPosition() { 
    return transform.position;
    }
    public bool IsEnemyDead()
    {
        return isDead;
    }


}
