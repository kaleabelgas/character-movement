using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    private Rigidbody2D gunRb;
    public GameObject gun;
    public Camera cam;

    public float moveSpeed;
    public float dashForce;
    public float dashTime;
    public float dashCDDefault;

    private float dashCD;
    private float playerAngle;
    private Vector2 lastDir = Vector2.down;
    private Vector2 moveDir = Vector2.zero;
    private Vector2 mousePos;
    private Vector2 lookDir;
    private bool isDashing = false;

    private void Awake()
    {
        dashCD = dashCDDefault;
        gunRb = gun.GetComponent<Rigidbody2D>();
        gunRb.isKinematic = false;


    }


    private void Update()
    {

        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (dashCD > 0)
        {
            dashCD -= Time.deltaTime;

        }

        //Debug.Log(dashCD);


        if (Mathf.Abs(moveDir.x) > 0.5f || Mathf.Abs(moveDir.y) > 0.5f)
        {
            lastDir = moveDir;
            Debug.Log("Setting both " + lastDir);
        }



        if (Input.GetButtonDown("Jump") && dashCD <= 0)
        {
            isDashing = true;
            Debug.Log("Jumped!" + lastDir);
        }

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);



    }

    private void FixedUpdate()
    {
        gunRb.MovePosition(transform.position);
        if (dashCD < dashTime)
        {
            Move();
        }

        if (isDashing)
        {
            StartCoroutine(Dash());
        }

        lookDir = mousePos - rb.position;

        playerAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        
        gunRb.rotation = playerAngle;


        Debug.Log("Velocity" + rb.velocity);
        Debug.Log("Direction" + lastDir);

    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            rb.AddForce(lastDir * dashForce * Time.deltaTime, ForceMode2D.Impulse);
            //transform.Translate(Vector2.Lerp(facing * dashforce, transform.position, Time.deltaTime));
            dashCD = dashCDDefault;
            yield return null;
        }
        isDashing = false;
        yield return null;

    }


    private void Move()
    {
        rb.velocity = moveSpeed * Time.deltaTime * moveDir;
        //transform.Translate(Vector2.Lerp(targetDirection * speed, transform.position, Time.deltaTime));
    }

}
