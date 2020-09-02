using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour
{
    public AnimationCurve lightcurve;
    float t;
    public Light light1;
    int delay = 3;
    bool lightup = false;
    AudioManager am;
    public bool easy = false;
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        t += Time.deltaTime;
        if (!lightup)
        {
            light1.intensity = 0;
            if (t > delay)
            {
                am.play("Thunder", 0.5f);
                if (easy)
                    delay = Random.Range(3, 6);
                else
                    delay = Random.Range(5, 10);
                t = 0;
                lightup = true;
            }
        }
        
        if (lightup)
        {
            light1.intensity = lightcurve.Evaluate(t*10) / 2;
            if (t > 0.5)
            {
                t = 0;
                lightup = false;
            }
        }
    }
}
