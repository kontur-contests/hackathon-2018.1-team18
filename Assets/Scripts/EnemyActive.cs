using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActive : MonoBehaviour {

    private readonly Tuple<float, float> movementRadius = new Tuple<float, float>(2, 2);
    private Vector2 startPosition;
    private Vector2 speed = new Vector2(6, 6);
    private Vector2 direction = new Vector2(1f, 0);
    private Vector2 movement = new Vector2();
    private Rigidbody2D rb;
    private bool playerNear;
    private EnemyBehaviour enemyBehaiviour = new EnemyBehaviour();
    private bool walkThrowStart;
    private float previousX;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = new Vector2(transform.position.x, transform.position.y);
        walkThrowStart = true;
    }

    // Update is called once per frame
    private void Update() {
        //if (playerNear)
        //  movement = new 
        
        movement = new Vector2(speed.x * direction.x, speed.y * direction.y);
        rb.velocity = movement;
        if ((transform.position.x >= startPosition.x + movementRadius.Item2
            || transform.position.x <= startPosition.x - movementRadius.Item1) && walkThrowStart)
        {
            direction = new Vector2(direction.x * (-1), direction.y);
            walkThrowStart = false;
        }
        if (previousX <= startPosition.x && transform.position.x >= startPosition.x
            || previousX >= startPosition.x && transform.position.x <= startPosition.x)
            walkThrowStart = true;
        previousX = transform.position.x;
    }
}
