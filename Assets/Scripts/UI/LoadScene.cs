using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string previewSceneName = "PreviewScene";

    void Start()
    {
        // Load the Preview Scene
        SceneManager.LoadScene(previewSceneName, LoadSceneMode.Additive);
    }
}
