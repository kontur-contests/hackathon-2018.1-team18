using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class MainCharacter : MonoBehaviour
{
    public float MovementSpeed = 10f;
    public float JumpSpeed = 10f;
    public float RepulsionSpeed = 10f;

    private int stage;
    private Transform renderers;
    private Transform cachedTransform;
    private Rigidbody2D rb;
    private Animator bodyAnim;
    private Animator flowerAnim;
    private int jumpCount = 0;
    private Vector3 startScale;
    private SpriteRenderer sp;
    private bool canMove = true;
    private bool transforming = false;
    private GameObject dustPrefab;
    private GameObject poofPrefab;
    private GameObject flushPrefab;


    private static readonly Dictionary<int, string> LastJumpFrame = new Dictionary<int, string>()
    {
        {1, "1_Body_Jump1_4" },
        {2, "2_Body_Jump1_10" }
    };

    private static readonly Dictionary<int, string> LastDoubleJumpAnim = new Dictionary<int, string>()
    {
        {2, "2_Body_Double1_6" }
    };

    private static readonly Dictionary<int, string> LastTransformFrame = new Dictionary<int, string>()
    {
        {1, "1_Body_Transform1_22" },
        {2, "Nothing" }
    };

    private const int DeathY = -5;
    private const float distanceToGroundLandingStart = 0.8f;

    private void Start()
    {
        cachedTransform = transform;
        rb = GetComponent<Rigidbody2D>();
<<<<<<< HEAD
        renderers = cachedTransform.Find("Renderers");  
=======
        renderers = cachedTransform.Find("Renderers");
>>>>>>> 1aeb1563279911741e2d55c3b0eb5f4de92b67b0
        bodyAnim = renderers.Find("Body").GetComponent<Animator>();
        flowerAnim = renderers.Find("Flower").GetComponent<Animator>();
        startScale = cachedTransform.localScale;
        sp = bodyAnim.GetComponent<SpriteRenderer>();
        dustPrefab = Resources.Load<GameObject>("Dust");
        poofPrefab = Resources.Load<GameObject>("Poof");
        flushPrefab = Resources.Load<GameObject>("Flush/Flush");
        stage = 1;
    }

    private void UpdateToNextStage()
    {
        stage++;
        renderers.position = new Vector2(renderers.position.x + 0.1f, renderers.position.y - 0.1f);
        bodyAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>($"{stage}_Body");
        flowerAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>($"{stage}_Flower");
    }

    private void Update()
    {
        if (cachedTransform.position.y < DeathY)
            SceneManager.LoadScene("DeathScene");
        if (Input.GetKeyDown(KeyCode.U) && stage == 1)
        {
            PlayAnim("transform");
            transforming = true;
            CreateEffect(flushPrefab);
        }

        else if (sp.sprite.name == LastTransformFrame[stage])
        {
            UpdateToNextStage();
            PlayAnim("transform");

        }

        if (transforming) return;

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canMove)
            {
                jumpCount = 1;
                PlayAnim("jump");
                canMove = false;
            }
            else if (stage > 1 && jumpCount == 1)
            {
                jumpCount = 2;
                PlayAnim("double");
                CreateEffect(poofPrefab);
            }
            return;
        }
        else if (sp.sprite.name == LastJumpFrame[stage])
        {
            rb.AddForce(new Vector2(0f, JumpSpeed));
            PlayAnim("inair");
            return;
        }
        else if (stage > 1 && sp.sprite.name == LastDoubleJumpAnim[stage])
        {
            rb.AddForce(new Vector2(0f, JumpSpeed));
            PlayAnim("inair");
            return;
        }
        
        var dir = Input.GetAxis("Horizontal");
        if (dir != 0)
            renderers.localScale = new Vector3(Mathf.Sign(dir) * startScale.x, startScale.y, startScale.z);
        var speedX = dir * MovementSpeed;
        rb.velocity = new Vector2(speedX, rb.velocity.y);
        if (canMove && jumpCount == 0)
            PlayAnim(dir == 0 ? "idle" : "walk");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumpCount != 0)
        {
            if (stage > 1 && collision.gameObject.tag == "Wall")
            {
                var replValue = RepulsionSpeed * Input.GetAxis("Horizontal");
                //rb.MovePosition(transform.position + new Vector3(-10f, 10f, 0f));
                rb.velocity += (new Vector2(-replValue, replValue));
            }

            else if(!canMove && collision.gameObject.tag == "Ground")
            {
                jumpCount = 0;
                canMove = true;
                CreateEffect(dustPrefab);
            }
        }
    }

    private void PlayAnim(string anim)
    {
        bodyAnim.Play(anim);
        flowerAnim.Play(anim);
    }

    private void CreateEffect(GameObject prefab)
    {
        var effect = Instantiate(prefab, transform);
        effect.transform.localPosition = prefab.transform.position;
        effect.transform.localScale = prefab.transform.localScale;
        effect.transform.parent = null;
    }
}
