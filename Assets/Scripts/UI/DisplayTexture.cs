using UnityEngine;
using UnityEngine.UI;

public class DisplayTexture : MonoBehaviour
{
    public RawImage rawImage;
    public string previewSceneName = "Preview Scene";

    void Start()
    {
        // Load the Preview Scene additively
        UnityEngine.SceneManagement.SceneManager.LoadScene(previewSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        // Find the RenderTexture and assign it to RawImage
        RenderTexture renderTexture = Resources.Load<RenderTexture>("EnvironmentRenderTexture");
        if (rawImage != null && renderTexture != null)
        {
            rawImage.texture = renderTexture;
        }
        else
        {
            Debug.LogError("RawImage or RenderTexture not assigned.");
        }
    }
}
