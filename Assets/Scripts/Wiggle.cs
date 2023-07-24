using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    /*
    I should be able to edit 
        the pivot (desireable)?
        The frequency
        The wavelength (ideally modified in degrees)
        And sustain (use an if statement) (modified as %)
    */

    //any number (related to degrees)
    public float frequency, amplitude; 
    //1 to 100
    public float sustain, peak;
    //Speed is how fast the counter increases as opposed to frequency
    public float counter, speed = 1;
    //Using FixedUpdate instead of Update fucked me
    private float fixedUpdateCorrector = 100;
    public bool stop = false;

    

    private void Update()
    {
        if (stop)
        {
            return;
        }
        //Debug.Log("r we paused");
        //Might not be radians atm
        counter += 1 * speed * fixedUpdateCorrector * Time.unscaledDeltaTime;
        float angle = Mathf.Sin(counter * frequency / 90);
        if (angle >= peak/100)
        {
            counter -= sustain/100*speed*Time.unscaledDeltaTime;
        }
        else if(angle <= -peak / 100)
        {
            counter -= sustain / 100*speed * Time.unscaledDeltaTime;
        }
        transform.rotation = new Quaternion(0f, 0f, amplitude*angle/100, 1f);
        //gameObject.GetComponent<Transform>().rotation.Set(0.4f,0f, angle, 1f);
    }
}
