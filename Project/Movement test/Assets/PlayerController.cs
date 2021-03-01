using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 1f;
    private Vector2 targetVelocity = Vector2.zero;
    public float dashforce = 50f;
    private Vector2 facing = Vector2.down;
    private bool dashing = false;
    public float dashTime = 10f;
    public float dashCDDefault;
    private float dashCD;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dashCD = dashCDDefault;

    }


    private void Update()
    {

        targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        dashCD -= Time.deltaTime;

        if  (Mathf.Abs(targetVelocity.x) > 0.5  || Mathf.Abs(targetVelocity.y) > 0.5)
        {
            facing = targetVelocity;
        }

        if (Input.GetButtonDown("Jump") && dashCD <= 0)
        {
            dashing = true;
            Debug.Log("Jumped!" + facing);
        }
    }

    private void FixedUpdate()
    {
        if (targetVelocity.x != 0 || targetVelocity.y != 0 && !dashing)
        {
            rb.AddForce(targetVelocity * speed * Time.deltaTime, ForceMode2D.Impulse);
        }

        if (dashing)
        {
            StartCoroutine(Dash());
            dashing = false;
        }

        Debug.Log("Velocity" + rb.velocity);
        Debug.Log("Direction" + facing);

    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            rb.AddForce(facing * dashforce, ForceMode2D.Impulse);
            dashCD = dashCDDefault;
            yield return null;
        }
    }

}
