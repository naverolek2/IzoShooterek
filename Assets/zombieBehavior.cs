using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class zombieBehavior : MonoBehaviour
{
    Rigidbody rb;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
     transform.LookAt(player.transform.position);   
     Vector3 playerDirection = transform.position - player.transform.position;
     transform.Translate(playerDirection.normalized * Time.deltaTime * 5);
    }
   
}
