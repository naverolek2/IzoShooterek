using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballController : MonoBehaviour
{
    public GameObject explosion;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            Destroy(explosion,5);
        }
    }
}
