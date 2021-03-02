using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb; // Get RigidBody of Object
    public GameObject gun; // Connect to gun GameObject
    public Camera cam; // Connect to camera
    private Rigidbody2D gunRb; // Initialize gun rigidbody

    public float moveSpeed; // Player speed
    public float dashForce; // amount of dash
    public float dashTime; // duration of dash - directly proportionate to dashforce;
    public float dashCDDefault; // Set default cd of dash

    private float dashCD; // Amount left on dash cooldown
    private float playerAngle; // stores angle of mouse relative to player

    private bool isDashing = false; // true if space is pressed

    private Vector2 lastDirection = Vector2.down; // stores last direction of player
    private Vector2 moveDirection = Vector2.zero; // vector2 of current movement input 
    private Vector2 mousePosition; // mouse position
    private Vector2 lookDirection; // angle of mouse relative to player, in vector2 direction

    private void Awake()
    {
        dashCD = dashCDDefault; // initializes dash cooldown to default
        gunRb = gun.GetComponent<Rigidbody2D>(); // gets rigidbody component of gun gameobject
        gunRb.isKinematic = false; // this is set to false so that gunrb.moveposition is instantaneous
    }


    private void Update()
    {
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // get movement input and save it to movedir vector2


        // this conditional is here so that dashcd doesnt go to absurd amounts below 0
        if (dashCD > 0)
        {
            // counts dashcd down in time
            dashCD -= Time.deltaTime;
        }

        // if either axis is moved, saves current direction vector as the last vector. necessary to dash while non moving.
        if (Mathf.Abs(moveDirection.x) > 0.5f || Mathf.Abs(moveDirection.y) > 0.5f)
        {
            lastDirection = moveDirection;
        }

        // sets jumping bool to true if space is pressed, and jump cooldown is over
        if (Input.GetButtonDown("Jump") && dashCD <= 0)
        {
            isDashing = true;
        }

        // converts mouse position in screen space to mouse position in world space and saves it to a variable
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        // moves the gun in relation to the player.
        gunRb.MovePosition(transform.position);

        // necessary so that the dash will complete before setting velocity/position. waits for dash cooldown to be less than the time it takes to dash
        if (dashCD < dashTime)
        {
            Move();
        }

        // if space is pressed, then call dash function. necessary to check for space press on Update() to not skip the frame that it is pressed.
        if (isDashing)
        {
            StartCoroutine(Dash());
        }

        lookDirection = mousePosition - rb.position; // sets the vector direction pointing to mouse by subtracting the two vector positions.
        playerAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f; // sets angle of the mouse in radians and offsets by 90 degrees
        gunRb.rotation = playerAngle; // sets rotation of the gun according to angle of mouse relative to player, in radians
    }

    IEnumerator Dash()
    {
        // initializes start time of dash 
        float startTime = Time.time;

        // run function while time is not yet reached 
        while (Time.time < startTime + dashTime)
        {
            // add a force based on last direction looked at, with dashForce amount of force, multiplied to Time.deltaTime so that it's constant.
            rb.AddForce(lastDirection * dashForce * Time.deltaTime, ForceMode2D.Impulse);
            // restarts dash cooldown
            dashCD = dashCDDefault;
            yield return null;
        }

        // this is outside the while loop so that we know to stop the function when the time specified in dashTime is donee
        isDashing = false;
        yield return null;
    }


    private void Move()
    {
        // moves the position according to current position modified by target move direction, multiplied to the speed and time.deltatime to stay constant.
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);
    }
}
