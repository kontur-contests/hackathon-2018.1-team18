using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

class MainCharacter : MonoBehaviour, IDamagable
{
    public float MovementSpeed = 10f;
    public float JumpSpeed = 10f;
    public float RepulsionSpeed = 10f;
    public int Damage = 10;

    public int HP
    {
        get { return hp;  }
        set
        {
            hp = value;
            if(hp <= 0)
                SceneManager.LoadScene("DeathScene");
        }
    }

    private int hp;
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
    private bool attacking = false;
    private GameObject currentWell = null;
    private GameObject dustPrefab;
    private GameObject poofPrefab;
    private GameObject flushPrefab;
    private GameObject blastPrefab;


    private static readonly Dictionary<int, Color> flushColor = new Dictionary<int, Color>()
    {
        {1, Color.white },
        {2, new Color32(200, 50, 200, 255)},
        {3, Color.red }
    };

    private static readonly Dictionary<int, string> lastJumpFrame = new Dictionary<int, string>()
    {
        {1, "1_Body_Jump1_4" },
        {2, "2_Body_Jump1_10" },
        {3, "3_Body_Jump1_15" }
    };

    private static readonly Dictionary<int, string> lastDoubleJumpAnim = new Dictionary<int, string>()
    {
        {2, "2_Body_Double1_6" }
    };

    private static readonly Dictionary<int, string> lastTransformStartFrame = new Dictionary<int, string>()
    {
        {1, "1_Body_Transform1_22" },
        {2, "2_Body_OutTransform2_23" },
        {3, "3_Flower_Walk1_0" },
        {4, "undefined" },
    };

    private static readonly Dictionary<int, string> lastTransformEndFrame = new Dictionary<int, string>()
    {
        {2, "2_Body_Transform2_14"},
        {3, "3_Body_InTransform2_8"},
        {4, "4_Body_Walk1_0" }
    };

    private static readonly Dictionary<int, string> applyAttackFrame = new Dictionary<int, string>()
    {
        {3, "3_Body_Attack2_13" },
        {4, "4_Body_Attack2_4" }
    };

    private static readonly Dictionary<int, string> endAttackFrame = new Dictionary<int, string>()
    {
        {3, "3_Body_Attack3_12" },
        {4, "4_Body_Attack2_22"}
    };

    private const int DeathY = -5;
    private const float distanceToGroundLandingStart = 0.8f;

    private void Start()
    {
        cachedTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        renderers = transform.Find("Renderers");
        bodyAnim = renderers.Find("Body").GetComponent<Animator>();
        flowerAnim = renderers.Find("Flower").GetComponent<Animator>();
        startScale = cachedTransform.localScale;
        sp = bodyAnim.GetComponent<SpriteRenderer>();
        dustPrefab = Resources.Load<GameObject>("Dust");
        poofPrefab = Resources.Load<GameObject>("Poof");
        flushPrefab = Resources.Load<GameObject>("Flush/Flush");
        blastPrefab = Resources.Load<GameObject>("Blast/Blast");
        stage = 1;
    }

    private void UpdateToNextStage()
    {
        stage++;
        if(stage == 2)
            renderers.position = new Vector2(renderers.position.x + 0.1f, renderers.position.y - 0.1f);
        bodyAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>($"{stage}_Body");
        flowerAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>($"{stage}_Flower");
    }

    private void Update()
    {
        if (cachedTransform.position.y < DeathY)
            HP = 0;

        if (attacking)
        {
            if (sp.sprite.name == endAttackFrame[stage])
                attacking = false;
            else if(sp.sprite.name == applyAttackFrame[stage])
            {
                if(stage == 3)
                    CreateEffect(blastPrefab).GetComponent<Blast>().Direction = Mathf.Sign(renderers.localScale.x);
                if(stage == 4)
                {
                    var collider = Physics2D.OverlapCircleAll(cachedTransform.position, 3f).
                        Where(g => g.tag == "Enemy").
                        FirstOrDefault();
                    if (collider != null)
                    {
                        var damagable = collider.GetComponent<IDamagable>();
                        if(damagable != null)
                            damagable.HP -= Damage;
                    }
                }
            }
            return;
        }
        if (canMove)
        {
            if (currentWell != null && Input.GetKeyDown(KeyCode.U) && stage < 4)
            {
                PlayAnim("outtransform");
                rb.velocity = Vector2.zero;
                transforming = true;
                Destroy(currentWell);
                CreateEffect(flushPrefab).GetComponent<SpriteRenderer>().color = flushColor[stage];
            }

            else if (sp.sprite.name == lastTransformStartFrame[stage])
            {
                UpdateToNextStage();
                PlayAnim("intransform");
            }

            else if (stage > 1 && sp.sprite.name == lastTransformEndFrame[stage])
                transforming = false;
        }
        if (transforming) return;

        else if (stage < 4 && Input.GetKeyDown(KeyCode.Space))
        {
            if (canMove)
            {
                jumpCount = 1;
                PlayAnim("jump");
                canMove = false;
            }
            else if (stage == 2 && jumpCount == 1)
            {
                jumpCount = 2;
                PlayAnim("double");
                CreateEffect(poofPrefab);
            }
            return;
        }
        else if(stage >= 3 && Input.GetKeyDown(KeyCode.E) && !attacking)
        {
            attacking = true;
            PlayAnim("attack");
        }
        else if (stage < 4 && sp.sprite.name == lastJumpFrame[stage])
        {
            rb.AddForce(new Vector2(0f, JumpSpeed));
            PlayAnim("inair");
            return;
        }
        else if (stage == 2 && sp.sprite.name == lastDoubleJumpAnim[stage])
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Well")
            currentWell = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Well")
            currentWell = null;
    }

    private void PlayAnim(string anim)
    {
        bodyAnim.Play(anim);
        flowerAnim.Play(anim);
    }

    private GameObject CreateEffect(GameObject prefab)
    {
        var effect = Instantiate(prefab, transform);
        effect.transform.localPosition = new Vector2(
            prefab.transform.position.x * Mathf.Sign(renderers.localScale.x),
            prefab.transform.position.y);
        effect.transform.localScale = prefab.transform.localScale;
        effect.transform.parent = null;
        return effect;
    }
}
