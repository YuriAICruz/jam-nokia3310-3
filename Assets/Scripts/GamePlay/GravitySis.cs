using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySis : MonoBehaviour
{
    private Rigidbody2D rbd;

    private Collider2D coll;

    private bool ground;

    public int jumpForce=20;
    public bool Player;
    // Start is called before the first frame update
    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")&& ground)
        {
            rbd.gravityScale *= -1;
        }

        if (Input.GetKeyDown("s")&& Player)
        {
            rbd.AddForce(transform.up*-jumpForce);
        }
        if (Input.GetKeyDown("w")&& Player)
        {
            rbd.AddForce(transform.up*jumpForce);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        ground = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        ground = false;
    }
}
