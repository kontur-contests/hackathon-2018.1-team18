using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RangeAttackEnemy : MonoBehaviour {

    public float damage = 1;
    private float distanceToPlayer;
    private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        distanceToPlayer = transform.position.x - 10;
        Destroy(gameObject, 10);
        rb.AddForce(new Vector2(distanceToPlayer*distanceToPlayer*distanceToPlayer, Math.Abs((float)Math.Pow(distanceToPlayer, 3))/2));
	}
	
}
