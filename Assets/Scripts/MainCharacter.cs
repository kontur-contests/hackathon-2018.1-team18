using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class MainCharacter : MonoBehaviour
{
    public float MovementSpeed = 10f;
    public float JumpSpeed = 10f;
    public float RepulsionSpeed = 10f;
    public int Stage
    {
        get { return stage; }
        set
        {
            stage = value;
            bodyAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>($"{stage}_Body");
            flowerAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>($"{stage}_Flower");
        }
    }

    private int stage;
    private Transform cachedTransform;
    private Collider2D cachedCollider;
    private Rigidbody2D rb;
    private Animator bodyAnim;
    private Animator flowerAnim;
    private int jumpCount = 0;
    private Vector3 startScale;
    private SpriteRenderer sp;
    private bool jumping = false;
    private bool canMove = true;
    private Vector2 startColliderOffset;
    private GameObject dustPrefab;


    private static readonly Dictionary<int, string> LastJumpFrame = new Dictionary<int, string>()
    {
        {1, "1_Body_Jump1_4" },
        {2, "2_Body_Jump1_10" }
    };

    private static readonly Dictionary<int, string> LastLandingFrame = new Dictionary<int, string>()
    {
        {1, "1_Body_Landing1_7"},
        {2, "2_Body_Landing1_10" }
    };

    private const int DeathY = -5;
    private const float distanceToGroundLandingStart = 0.5f;

    private void Start()
    {
        cachedTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        bodyAnim = cachedTransform.Find("Renderers").Find("Body").GetComponent<Animator>();
        flowerAnim = cachedTransform.Find("Renderers").Find("Flower").GetComponent<Animator>();
        startScale = cachedTransform.localScale;
        sp = bodyAnim.GetComponent<SpriteRenderer>();
        cachedCollider = GetComponent<Collider2D>();
        startColliderOffset = cachedCollider.offset;
        dustPrefab = Resources.Load<GameObject>("Dust");
        Stage = 1;
    }

    private void Update()
    {
        if (cachedTransform.position.y < DeathY)
            SceneManager.LoadScene("DeathScene");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canMove)
            {
                PlayAnim("jump");
                jumping = true;
                canMove = false;
            }
            return;
        }
        if(sp.sprite.name == LastJumpFrame[Stage])
        {
            rb.AddForce(new Vector2(0f, JumpSpeed));
            cachedCollider.offset = new Vector2(startColliderOffset.x, startColliderOffset.y - distanceToGroundLandingStart);
            cachedCollider.isTrigger = true;
            PlayAnim("inair");
            jumping = false;
            return;
        }
        if (sp.sprite.name == LastLandingFrame[Stage])
            canMove = true;
        if (canMove && jumpCount == 0)
        {
            var dir = Input.GetAxis("Horizontal");
            if (dir != 0)
                cachedTransform.localScale = new Vector3(Mathf.Sign(dir) * startScale.x, startScale.y, startScale.z);
            var speedX = dir * MovementSpeed;
            PlayAnim(dir == 0 ? "idle" : "walk");
            rb.velocity = new Vector2(speedX, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!jumping && jumpCount != 0)
        {
            if (Stage > 1 && collision.gameObject.tag == "Wall")
            {
                var replValue = RepulsionSpeed * Input.GetAxis("Horizontal");
                rb.AddForce(new Vector2(-replValue, replValue));
            }

            else if(collision.gameObject.tag == "Ground")
            {
                jumpCount = 0;
                var dust = Instantiate(dustPrefab, transform);
                dust.transform.localPosition = dustPrefab.transform.position;
                dust.transform.localScale = dustPrefab.transform.localScale;
                dust.transform.parent = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!jumping && jumpCount != 0 && collision.gameObject.tag == "Ground")
        {
            PlayAnim("landing");
            cachedCollider.offset = startColliderOffset;
            cachedCollider.isTrigger = false;
        }
    }

    private void PlayAnim(string anim)
    {
        bodyAnim.Play(anim);
        flowerAnim.Play(anim);
    }
}
