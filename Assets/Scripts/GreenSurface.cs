using System.Collections;
using System.Linq;
using UnityEngine;

class GreenSurface : MonoBehaviour
{
    private void Start() => StartCoroutine(StartCor());

    private IEnumerator StartCor()
    {
        Destroy(GetComponent<Rigidbody2D>());
        var objects = GameObject.Find("Level1_Green").transform.Cast<Transform>().Reverse();
        foreach(var obj in objects)
        {
            obj.GetComponent<Animator>().Play("green");
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        transform.position += Vector3.left * 0.3f;
    }
}