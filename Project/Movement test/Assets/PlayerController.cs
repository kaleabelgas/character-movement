using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 0f;
    private Vector2 targetVelocity = Vector2.zero;
    public float dashforce = 50f;
    private Vector2 facing = Vector2.zero;
    private bool dashing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {

        targetVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical")) * speed;

        if  (targetVelocity.x != 0  || targetVelocity.y != 0)
        {
            facing = new Vector2 (Mathf.Clamp(targetVelocity.x, -1, 1), Mathf.Clamp(targetVelocity.y, -1, 1));
            Debug.Log(facing);
        }

        if (Input.GetButtonDown("Jump"))
        {
            dashing = true;
            Debug.Log("Jumped!" + facing);
        }

    }

    private void FixedUpdate()
    {
        if (targetVelocity.x != 0 || targetVelocity.y != 0 && !dashing)
        {
            rb.velocity = targetVelocity * Time.deltaTime;
        }

        if (dashing)
        {
            rb.velocity = facing * dashforce;
            dashing = false;
        }



    }

}
