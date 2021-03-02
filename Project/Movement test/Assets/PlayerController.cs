using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 1f;
    public float dashForce = 50f;
    public float dashTime = 10f;
    public float dashCDDefault;

    private float dashCD;
    private Vector2 facing = Vector2.down;
    private Vector2 targetDirection = Vector2.zero;
    private bool dashing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dashCD = dashCDDefault;

    }


    private void Update()
    {

        targetDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if (dashCD > 0)
        {
            dashCD -= Time.deltaTime;

        }

        Debug.Log(dashCD);


        if (Mathf.Abs(targetDirection.x) > 0.5f || Mathf.Abs(targetDirection.y) > 0.5f)
        {
            facing = targetDirection;
            //Debug.Log("Setting both " + facing);
        }



        if (Input.GetButtonDown("Jump") && dashCD <= 0)
        {
            dashing = true;
            Debug.Log("Jumped!" + facing);
        }
    }

    private void FixedUpdate()
    {
        if (dashCD < dashTime)
        {
            Move();
        }

        if (dashing)
        {
            StartCoroutine(Dash());
        }

        //Debug.Log("Velocity" + rb.velocity);
        //Debug.Log("Direction" + facing);

    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            rb.AddForce(facing * dashForce * Time.deltaTime, ForceMode2D.Impulse);
            //transform.Translate(Vector2.Lerp(facing * dashforce, transform.position, Time.deltaTime));
            dashCD = dashCDDefault;
            yield return null;
        }
        dashing = false;
        yield return null;

    }


    private void Move()
    {
        rb.velocity = speed * Time.deltaTime * targetDirection;
        //transform.Translate(Vector2.Lerp(targetDirection * speed, transform.position, Time.deltaTime));
    }

}
