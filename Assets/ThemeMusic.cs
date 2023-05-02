using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeMusic : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource source;
    public AudioClip clip;


    void Start()
    {
        source.clip = clip;
        source.volume = 0.45f;
        source.Play();
        source.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
