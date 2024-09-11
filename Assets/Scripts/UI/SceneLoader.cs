using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string previewSceneName = "PreviewScene";

    void Start()
    {
        // Load the Preview Scene additively
        SceneManager.LoadScene(previewSceneName, LoadSceneMode.Additive);
    }
}
