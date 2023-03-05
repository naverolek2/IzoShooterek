using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    Vector2 inputVector;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputVector = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Wychylenie kontrolera" + inputVector.ToString());
        //Vector3 movement = new Vector3(0,0, inputVector.y);
        //bez fizyki
        //transform.Translate(movement);
        //Vector3 rotation = new Vector3(0, inputVector.x, 0);
         //transform.Rotate(rotation);
    }
    private void FixedUpdate()
    {
        //z fizyk¹
        if(inputVector.y == 0 ) { 
            rb.velocity = Vector2.zero;
        }
        else
        {
        Vector3 movement = transform.forward * inputVector.y;
        rb.AddForce(movement, ForceMode.Impulse);
        }

        if (inputVector.x == 0)
        {
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            Vector3 rotation = transform.up * inputVector.x;
            rb.AddTorque(rotation, ForceMode.Impulse);
        }




        
    }
    void OnMove(InputValue inputValue) 
    {
        inputVector = inputValue.Get<Vector2>();
        
    }
}
