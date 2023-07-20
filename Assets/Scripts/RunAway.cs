using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAway : MonoBehaviour
{
    public GameObject Player, PlayingField;
    public Rigidbody2D rb;
    public float radiusMultiplier, speed, ignore, stamina;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //I have got to just normalize this curse you chatgtp + my laziness

        //point away from player
        Vector2 direction = Player.transform.position - transform.position;

        float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;

        // Create a quaternion based on the rotation angle
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Point to centre
        Vector2 directionToCentre = PlayingField.transform.position - transform.position;

        float angleToCentre = Mathf.Atan2(directionToCentre.y, directionToCentre.x) * Mathf.Rad2Deg;

        Quaternion targetRotationToCentre = Quaternion.AngleAxis(angleToCentre, Vector3.forward);

        Vector2 velocityAway = targetRotation * Vector2.right;
        Vector2 velocityHome = targetRotationToCentre * Vector2.right;

        float hooHaMath = 1/direction.magnitude;

        //STAMINA - enemy loses stamina if player is close by.
        if (hooHaMath >= 1.05 && stamina > 0.8f)
        {
            stamina -= 0.0005f;
            //Add particles here soon
        }

        if ((velocityHome.normalized + velocityAway.normalized).magnitude <= 0.01f)
        {
            rb.velocity = Vector2.zero;
            Debug.Log("Stop wiggles");
            return;
        }
        //determines enemy direction + speed
        rb.velocity =  (velocityHome * radiusMultiplier * directionToCentre.magnitude + velocityAway * ignore ).normalized * speed * hooHaMath * stamina;

        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * 1f,  transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1f, transform.localScale.y, transform.localScale.z);
        }

        //Vector directions used to calculate direction
        Debug.DrawRay(transform.position, rb.velocity);
        Debug.DrawRay(transform.position, directionToCentre);
    }
}
