using UnityEngine;

class Dust : MonoBehaviour
{
    private const string LastFrame = "Dust_3";

    private SpriteRenderer sp;

    private void Start() => sp = GetComponent<SpriteRenderer>();

    private void Update()
    {
        if (sp.sprite.name == LastFrame)
            Destroy(gameObject);
    }
}