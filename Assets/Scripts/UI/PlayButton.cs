using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class PlayButton : MonoBehaviour
{
    private void Start() => GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("Level1"));
}