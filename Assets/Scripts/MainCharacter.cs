using UnityEngine;
using UnityEngine.SceneManagement;

class MainCharacter : MonoBehaviour
{
    public float MovementSpeed = 10f;
    public float JumpSpeed = 10f;
    public float RepulsionSpeed = 10f;
    public int Stage = 1;

    private Transform cachedTransform;
    private Rigidbody2D rb;
    private int jumpCount = 0;
    private const int deathY = -4;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cachedTransform = transform;
    }

    private void Update()
    {
        var speedY = 0f;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount++ < Stage)
                speedY = JumpSpeed;
        }
<<<<<<< HEAD
        rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * MovementSpeed, speedY));
        if (cachedTransform.position.y < deathY)
=======
        var speedX = jumpCount == 0 ? Input.GetAxis("Horizontal") * MovementSpeed : 0f;
        rb.AddForce(new Vector2(speedX, speedY));
        if (cachedTransform.position.y < 0)
>>>>>>> feeb3f0c6c8a9810281ab5c1749c67c1f863141f
            SceneManager.LoadScene("DeathScene");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumpCount != 0)
        {
            if (collision.gameObject.tag == "Ground")
                jumpCount = 0;
            else if (Stage > 1 && collision.gameObject.tag == "Wall")
            {
                var replValue = RepulsionSpeed * Input.GetAxis("Horizontal");
                rb.AddForce(new Vector2(-replValue, replValue));
            }
        }
    }

}
