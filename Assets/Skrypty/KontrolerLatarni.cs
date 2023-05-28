using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KontrolerLatarni : MonoBehaviour
{


    public Light _light;

    public float MinTime;
    public float MaxTime;
    public float Timer;

    
    // Start is called before the first frame update
    void Start()
    {
        MinTime = 0.1f;
        MaxTime = 0.5f;
        Timer = Random.Range(MinTime, MaxTime); 
    }

    // Update is called once per frame
    void Update()
    {
        swiatloEfekt();
    }

    void swiatloEfekt()
    {
        if(Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        if(Timer <= 0)
        {
            _light.enabled = !_light.enabled;
            Timer = Random.Range(MinTime, MaxTime);

        }
    }
}
