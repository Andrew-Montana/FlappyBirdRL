using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private float force;
    private bool isDead;
    private float counter = 0;

    private Rigidbody2D rb;
    private Animator animator;

    private AudioClip audioClip;
    private AudioClip smashClip;
    private AudioSource audioSource;
    private bool first = false;
    private bool isSmashed = false;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isDead = false;
        force = 240;
        audioClip = Resources.Load("Sounds/birdflap2") as AudioClip;
        smashClip = Resources.Load("Sounds/smash") as AudioClip;
        audioSource.clip = audioClip;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {
            if(counter == 0)
            {
                animator.SetInteger("State", 0);

                if (Input.GetMouseButton(0))
                {
                    audioSource.PlayOneShot(audioClip, 0.2f);
                    rb.velocity = Vector2.zero;
                    rb.AddForce(new Vector2(0, force));
                    animator.SetInteger("State", 1);
                    counter = 1 ;
                }
            }
            if (counter > 0)
                counter += Time.deltaTime;

            if (counter >= 1.5f)
                counter = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isDead == false)
        {
            rb.velocity = Vector2.zero;
            isDead = true;
            animator.SetInteger("State", 2);
            GameController.instance.GameOver();
        }
        if(collision.gameObject.tag == "Ground")
        {
            if(isSmashed == false)
            {
                audioSource.PlayOneShot(smashClip, 0.2f);
                isSmashed = true;
            }
        }
    }
}
