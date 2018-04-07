using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RangeAttackEnemy : MonoBehaviour {

    private Transform playerTransform;
    public float damage = 1;
    private float distanceToPlayer;
    private Rigidbody2D rb;
    // Use this for initialization
    void Start()
    {
        playerTransform = FindObjectOfType<MainCharacter>().transform;
        rb = GetComponent<Rigidbody2D>();
        distanceToPlayer = transform.position.x - 10;
        rb.AddForce(new Vector2(distanceToPlayer * distanceToPlayer * distanceToPlayer, Math.Abs((float)Math.Pow(distanceToPlayer, 3)) / 2));
    }

    public float Speed;
    public float MinDist = 2f;
    public int Damage;
    public Transform RotTransform;

    private readonly static Vector2 rotVect = new Vector2(0f, 20f);
    private void Update()
    {
        if (playerTransform != null && (playerTransform.position - transform.position).magnitude <= MinDist)
        {
            gameObject.SetActive(false);
        }
        var targetVector = playerTransform.position - transform.position;
        targetVector = new Vector2(targetVector.x, targetVector.y);
        if (targetVector.magnitude <= MinDist)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (RotTransform != null)
                RotTransform.Rotate(rotVect);
            transform.Translate(targetVector.normalized * Speed * Time.deltaTime);
        }
    }

}
