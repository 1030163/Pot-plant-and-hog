using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    private Wiggle wiggle;
    public int MovementSpeed = 1;
    public BasicInkExample basicInk;
    public List<TextAsset> chapters;
    public RunAway Hog;

    private Vector2 MovementDirection;
    private Vector2 moveAngleQuaternion;
    private int iFrames = 0;

    public GameObject LeftWall, RightWall, Roof, Floor;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wiggle = GetComponent<Wiggle>();
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
        if (collision.gameObject.tag == "Opponent" && iFrames > 100)
        {
            iFrames = 0;
            Hog.stamina += 0.4f;
            Debug.Log("Enemy tagged");
            basicInk.StartStory(chapters[0]);
            chapters.Remove(chapters[0]);
        }
    }


    private void FixedUpdate()
    {
        iFrames += 1;
        int Moving;
        if (MovementDirection != new Vector2(0,0)) {
            Moving = 1;
            wiggle.stop = false;
        }
        else
        {
            Moving = 0;
            wiggle.stop = true;
        }

        rb.velocity = Moving * MovementSpeed * new Vector2(0.05f, 0.05f) * moveAngleQuaternion + 0.6f * rb.velocity;

        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * 1f, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1f, transform.localScale.y, transform.localScale.z);
        }

        MovementDirection = new Vector2(0,0);
    }
}
