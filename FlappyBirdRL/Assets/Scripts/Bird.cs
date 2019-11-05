using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float horizontalDistance;
    public float verticalDistance;

    private float force;
    public bool isDead;
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
        //   Vector2 forward = //transform.TransformDirection(Vector2.right) * 6;
        // Debug.DrawLine(transform.position, transf, Color.red, 0.5f, false);
        int layerMask = 1 << 8;
       // Debug.DrawLine(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), new Vector3(7,transform.position.y,0), Color.red, 0.5f, false);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector3(7, transform.position.y, 0), 7, layerMask);
        if (hit.collider != null)
        {
            GameObject lowerPipe = hit.transform.GetChild(0).gameObject;
            GameObject higherPipe = hit.transform.GetChild(1).gameObject;
            horizontalDistance = Vector3.Distance(transform.position, hit.transform.position);
            // Debug.Log(lowerPipe.transform.position.y);
            Debug.DrawLine(new Vector3(hit.transform.position.x, lowerPipe.transform.position.y + (ColumnPool.heightOfPipe/2) ,0), new Vector3(hit.transform.position.x, 3, 0), Color.red, 0.5f, false);
            Debug.DrawLine(transform.position, new Vector3(horizontalDistance,transform.position.y,0), Color.green, 0.5f, false);
        }
            if (!isDead)
        {
            if(counter == 0)
            {
                animator.SetInteger("State", 0);

                if (Input.GetMouseButton(0)) //(Bot.isJump)//(Input.GetMouseButton(0))
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
