using UnityEngine;

class Wall : MonoBehaviour, IDamagable
{
    public int HP
    {
        get
        {
            return 0;
        }

        set
        {
            anim.Play("destroy");
        }
    }

    private Animator anim;

    private void Start() => anim = GetComponent<Animator>();

}