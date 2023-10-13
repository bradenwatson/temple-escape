using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterNextLevelControl : MonoBehaviour
{
    [SerializeField]
    private int _nextSceneIndex;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SceneManager.LoadScene(_nextSceneIndex);
        }
    }
}
