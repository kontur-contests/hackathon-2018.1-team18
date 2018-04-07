using UnityEngine;

class MainCharacter : MonoBehaviour
{
    public float MovementSpeed = 10f;
    public float JumpSpeed = 10f;

    //private Transform cachedTransform;
    private Rigidbody2D rb;

    private void Start() => rb = GetComponent<Rigidbody2D>();

    private void Update()
    {
        var speedY = Input.GetKeyDown(KeyCode.Space) ? JumpSpeed : 0f;
        rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * MovementSpeed, speedY));
    }

}
