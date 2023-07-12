using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public int MovementSpeed = 1;

    private Vector2 MovementDirection;
    private Vector2 moveAngleQuaternion;

    public GameObject LeftWall, RightWall, Roof, Floor;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            MovementDirection.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            MovementDirection.y += -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            MovementDirection.x += -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            MovementDirection.x += 1;
        }
        float moveAngle = Mathf.Atan2(MovementDirection.y, MovementDirection.x) * Mathf.Rad2Deg;

        moveAngleQuaternion = Quaternion.AngleAxis(moveAngle, Vector3.forward) * Vector2.right;

    }

    /*
    Psudocode / thoughts
    player has set speed (no up + right combo speed bullshit)
    enemies have a set speed slightly slower than the player
    enemies move faster than the player when close, this effect dampens by the player being close
    *done* enemies direct away from the player and lean further tangential to the centre of the arena the further out they go from it.
     */


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Opponent")
        {
            Debug.Log("Enemy tagged");
        }
    }


    private void FixedUpdate()
    {
        int Moving;
        if (MovementDirection != new Vector2(0,0)) {
            Moving = 1;
        }
        else
        {
            Moving = 0;
        }

        rb.velocity = Moving * MovementSpeed * new Vector2(0.05f, 0.05f) * moveAngleQuaternion + 0.6f * rb.velocity;
        MovementDirection = new Vector2(0,0);
    }
}
