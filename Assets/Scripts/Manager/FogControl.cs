using UnityEngine;

public class FogController : MonoBehaviour
{
    public float minFogDensity = 0.01f;
    public float maxFogDensity = 0.05f;
    public float fogChangeSpeed = 0.5f;

    private float fogDirection = 0.1f;

    void Start()
    {
        RenderSettings.fog = true;
    }

    void Update()
    {
        RenderSettings.fogDensity += fogChangeSpeed * fogDirection * Time.deltaTime;

        if (RenderSettings.fogDensity >= maxFogDensity)
        {
            fogDirection = -1f;
        }
        else if (RenderSettings.fogDensity <= minFogDensity)
        {
            fogDirection = 1f;
        }
    }
}
