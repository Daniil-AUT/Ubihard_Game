using UnityEngine;

public class CameraRenderSetUp : MonoBehaviour
{
    public RenderTexture renderTexture;

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        if (camera != null && renderTexture != null)
        {
            camera.targetTexture = renderTexture;
        }
        else
        {
            Debug.LogError("Camera or RenderTexture not assigned.");
        }
    }
}
