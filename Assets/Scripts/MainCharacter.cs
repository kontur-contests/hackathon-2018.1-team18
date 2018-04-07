using UnityEngine;
using UnityEngine.SceneManagement;

class MainCharacter : MonoBehaviour
{
    public float MovementSpeed = 10f;
    public float JumpSpeed = 10f;
    public int Stage = 1;

    private Transform cachedTransform;
    private Rigidbody2D rb;
    private int jumpCount = 0;

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
        rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * MovementSpeed, speedY));
        if (cachedTransform.position.y < 0)
            SceneManager.LoadScene("DeathScene");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumpCount != 0 && collision.gameObject.tag == "Ground")
            jumpCount = 0;
    }

}
