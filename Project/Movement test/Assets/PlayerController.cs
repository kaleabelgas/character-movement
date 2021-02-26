using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 40f;
    private float horDir = 0f;
    private float verDir = 0f;
    private Vector3 direction = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        horDir = Input.GetAxisRaw("Horizontal");
        verDir = Input.GetAxisRaw("Vertical");

        Debug.Log(horDir);
        Debug.Log(verDir);

    }


    private void FixedUpdate()
    {
        rb.velocity = (speed * Time.fixedDeltaTime * new Vector2(horDir, verDir));

    }
}
