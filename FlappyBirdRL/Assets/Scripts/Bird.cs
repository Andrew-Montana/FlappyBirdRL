using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private float force;
    private bool isDead;

    private Rigidbody2D rb;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isDead = false;
        force = 200f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {
            animator.SetInteger("State", 0);

            if (Input.GetMouseButton(0))
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(0, force));
                animator.SetInteger("State", 1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isDead = true;
        animator.SetInteger("State", 2);
    }
}
