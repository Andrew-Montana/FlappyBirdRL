using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Columns : MonoBehaviour
{
    private Rigidbody2D rb;
    private float scrollSpeed = -2.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(scrollSpeed, 0);
    }

    private void Update()
    {
        if (GameController.instance.gameOver)
            rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.tag == "Player")
        {
            GameController.instance.GetScore();
            Bot.isPassed = true;
        }
    }
}
