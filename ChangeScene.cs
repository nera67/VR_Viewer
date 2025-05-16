using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    public string SetSceneName(string sceneName) => _sceneName = sceneName;

    public void Change()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
