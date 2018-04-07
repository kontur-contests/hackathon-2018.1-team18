using UnityEngine;
using UnityEngine.SceneManagement;

class MainCharacter : MonoBehaviour
{
    public float MovementSpeed = 10f;
    public float JumpSpeed = 10f;

    private Transform cachedTransform;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cachedTransform = transform;
    }

    private void Update()
    {
        var speedY = Input.GetKeyDown(KeyCode.Space) ? JumpSpeed : 0f;
        rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * MovementSpeed, speedY));
        if (cachedTransform.position.y < 0)
            SceneManager.LoadScene("DeathScene");
    }

}
