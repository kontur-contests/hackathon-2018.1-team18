using UnityEngine;

class MainCharactercher : MonoBehaviour
{
    private Transform cachedTransform;

    private void Start() => cachedTransform = transform;

}
