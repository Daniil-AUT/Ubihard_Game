using UnityEngine;

public class CameraRenderSetUp : MonoBehaviour
{
    public RenderTexture renderTexture;

    // used to render camera view to a texture to show a preview of game
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        if (camera != null && renderTexture != null)
        {
            camera.targetTexture = renderTexture;
        }
        else
        {
            Debug.LogError("no camera - can't see the view");
        }
    }
}
