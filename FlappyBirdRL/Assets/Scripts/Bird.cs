using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private float force;
    private Rigidbody2D rb;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isDead = false;
        force = 200f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, force));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isDead = true;
    }
}
