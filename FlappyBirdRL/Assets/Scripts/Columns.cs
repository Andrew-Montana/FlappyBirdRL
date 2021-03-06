﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Columns : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject player;
    private Bird bird;
    private float scrollSpeed = -2.5f;
    private Vector3 localPos;
    private string name;

    private void Start()
    {
        name = transform.gameObject.name.ToString();
        localPos = transform.localPosition;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Bird");
        bird = player.GetComponent<Bird>();
        rb.velocity = new Vector2(scrollSpeed, 0);
    }

    private void Update()
    {
        if (bird.isDead)
            transform.localPosition = localPos;

        if (bird.isEndState)
            transform.localPosition = localPos;
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.tag == "Player")
        {
            //  GameController.instance.GetScore();
            bird.isTriggered = true;
            if(name == "EndState")
            {
                Debug.Log(name);
                bird.isEndState = true;
            }
        }
    }
}
