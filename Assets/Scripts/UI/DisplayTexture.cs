using UnityEngine;
using UnityEngine.UI;

public class DisplayTexture : MonoBehaviour
{
    public RawImage rawImage;
    public string previewSceneName = "Preview Scene";

    void Start()
    {
    // Set the image of the ui to the render texture from the Preview Scene to be played
    // alongside the  starting screen for a aesthetic look
        UnityEngine.SceneManagement.SceneManager.LoadScene(previewSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        RenderTexture renderTexture = Resources.Load<RenderTexture>("EnvironmentRenderTexture");
        if (rawImage != null && renderTexture != null)
        {
            rawImage.texture = renderTexture;
        }
        else
        {
            Debug.LogError("The preview of the scene is not available");
        }
    }
}
