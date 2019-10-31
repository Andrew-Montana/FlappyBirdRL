using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D rb;
    private float scrollSpeed = -5.5f;
    private BoxCollider2D boxCol;
    private float offset;

    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        offset = boxCol.size.x * 2f;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(scrollSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.gameOver)
            rb.velocity = Vector2.zero;

        if(transform.position.x < -boxCol.size.x)
        {
            RepositionGround();
        }
    }

    private void RepositionGround()
    {
        transform.position = transform.position + new Vector3(offset,0,0);
    }
}
