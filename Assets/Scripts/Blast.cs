using UnityEngine;

class Blast : MonoBehaviour
{
    public int Damage = 30;
    public float Speed = 10;
    public float Direction = 1f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(Direction * Speed, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        var damagable = collision.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
            damagable.HP -= Damage;
    }
}