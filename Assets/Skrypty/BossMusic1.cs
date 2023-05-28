using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic1 : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    float timePassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > 2f)
        {

            source.PlayOneShot(clip, 0.2f);
            timePassed = -10410f;

        }

    }
}
