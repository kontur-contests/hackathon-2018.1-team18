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
    private Animator bodyAnim;
    private Animator flowerAnim;
    private int jumpCount = 0;
    private Vector3 startScale;
    private SpriteRenderer sp;

    private const int DeathY = -4;
    private const string LastJumpFrame = "1_Body_Jump1_4";

    private void Start()
    {
        cachedTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        bodyAnim = cachedTransform.Find("Renderers").Find("Body").GetComponent<Animator>();
        flowerAnim = cachedTransform.Find("Renderers").Find("Flower").GetComponent<Animator>();
        startScale = cachedTransform.localScale;
        sp = bodyAnim.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount++ < Stage)
                PlayAnim("jump");
            return;
        }
        if(sp.sprite.name == LastJumpFrame)
        {
            rb.AddForce(new Vector2(0f, JumpSpeed));
            PlayAnim("inair");
            return;
        }
        if (jumpCount == 0)
        {
            var dir = Input.GetAxis("Horizontal");
            if (dir != 0)
                cachedTransform.localScale = new Vector3(Mathf.Sign(dir) * startScale.x, startScale.y, startScale.z);
            var speedX = dir * MovementSpeed;
            PlayAnim(dir == 0 ? "idle" : "walk");
            rb.velocity = new Vector2(speedX, rb.velocity.y);
            if (cachedTransform.position.y < DeathY)
                SceneManager.LoadScene("DeathScene");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumpCount != 0)
        {
            if (collision.gameObject.tag == "Ground")
            {
                jumpCount = 0;
                PlayAnim("landing");
            }
            else if (Stage > 1 && collision.gameObject.tag == "Wall")
            {
                var replValue = RepulsionSpeed * Input.GetAxis("Horizontal");
                rb.AddForce(new Vector2(-replValue, replValue));
            }
        }
    }

    private void PlayAnim(string anim)
    {
        bodyAnim.Play(anim);
        flowerAnim.Play(anim);
    }
}
