using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;


public class cameraController : NetworkBehaviour
{ 
    
    public Vector3 playerOffset;
    GameObject player;
    public float smoothTime = 0.2f;
    private Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPostion = player.transform.position + playerOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPostion, ref velocity, smoothTime);
        
    }
    

}
