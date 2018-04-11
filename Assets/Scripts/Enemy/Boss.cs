using System;
using UnityEngine;

public class Boss : MonoBehaviour, IDamagable
{
    private Transform playerTransform;
    private Transform cachedTransform;
    private Rigidbody2D rb;
    private Animator bossAnim;
    private float startX;
    private SpriteRenderer sp;
    private bool attacking = false;
    private bool dying = false;
    private int hp = 0;

    private void Start()
    {
        bossAnim = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        cachedTransform = transform;
        startX = cachedTransform.position.x;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    private const float AgrDistance = 30f;
    private const float WalkingDistance = 10f;
    private const float AttackingDistance = 1f;
    private const string AttackFrame = "boss_atk3_7";
    private const string EndAttackFrame = "boss_atk3_19";
    private const string EndDeathFrame = "boss_deth3_19";

    public float Speed = 5;
    public int Damage = 50;
    
    public int HP
    {
        get { return hp;  }
        set
        {
            hp = value;
            if (hp <= 0)
            {
                bossAnim.Play("death");
                dying = true;
            }
        }
    }
   
    private void Update()
    {
        if(dying)
        {
            if (sp.sprite.name == EndDeathFrame)
                Destroy(gameObject);
            return;
        }
        /*if(attacking)
        {
            if (sp.sprite.name == AttackFrame)
                playerTransform.gameObject.GetComponent<IDamagable>().HP -= Damage;
            else if (sp.sprite.name == EndAttackFrame)
                attacking = false;
            return;
        }*/
        var delta = cachedTransform.position.x - playerTransform.position.x;
        /*if(Math.Abs(delta) <= AttackingDistance)
        {
            bossAnim.Play("attack");
            attacking = true;
        }*/
        if (Mathf.Abs(delta) <= AgrDistance)
        {
            Speed = Math.Sign(delta) * Speed;
            cachedTransform.localScale = new Vector2(Math.Sign(delta) * cachedTransform.localScale.x, cachedTransform.localScale.y);
        }
        else if (Mathf.Abs(startX - cachedTransform.position.x) >= WalkingDistance)
        {
            Speed = -Speed;
            cachedTransform.localScale = new Vector2(-cachedTransform.localScale.x, cachedTransform.localScale.y);
        }
        rb.velocity = new Vector2(Speed, 0f);
        bossAnim.Play("walk");
    }
}
