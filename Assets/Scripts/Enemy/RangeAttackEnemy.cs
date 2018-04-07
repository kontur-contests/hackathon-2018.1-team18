using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RangeAttackEnemy : MonoBehaviour
{

    private Transform playerTransform;
    public float damage = 1;
    private float distanceToPlayer;
    private Rigidbody2D rb;
    private float speed;
    private Animator Shell;
    // Use this for initialization
    void Start()
    {
        Shell = GetComponent<Animator>();
        speed = 300;
        playerTransform = FindObjectOfType<MainCharacter>().transform;
        rb = GetComponent<Rigidbody2D>();
        distanceToPlayer = playerTransform.position.x - transform.position.x;
        //rb.AddForce(new Vector2(distanceToPlayer * distanceToPlayer * distanceToPlayer, Math.Abs((float)Math.Pow(distanceToPlayer, 3)) / 2));
    }

    public float MinDist = 2f;
    public int Damage;
    public Transform RotTransform;
    private float walkedPath;

    private readonly static Vector2 rotVect = new Vector2(0f, 20f);
    private void Update()
    {
        Attack();
    }

    public void Attack()
    {
        if (playerTransform != null && (playerTransform.position - transform.position).magnitude <= MinDist)
        {
            gameObject.SetActive(false);
        }
        var targetVector = playerTransform.position - transform.position;
        if (Math.Abs(walkedPath) < Math.Abs(distanceToPlayer))
            targetVector.y = 10;
        walkedPath += targetVector.normalized.x * speed * Time.deltaTime;
        targetVector = new Vector2(targetVector.x, targetVector.y);
        if (targetVector.magnitude <= MinDist)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (RotTransform != null)
                RotTransform.Rotate(rotVect);
            rb.velocity = targetVector.normalized * speed * Time.deltaTime;
        }
    }

}