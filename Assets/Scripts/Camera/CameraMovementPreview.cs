using UnityEngine;

public class CameraHorizontalRotation : MonoBehaviour
{
    public float rotationSpeed = 30f; 
    public float delay = 2f; 

    private Quaternion startRotation; 
    private Quaternion endRotation; 
    private bool rotatingToEnd = true; 
    private float rotationProgress = 0f; 

    void Start()
    { 
        // get the end rotation 
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 45, 0));
    }
    
    void Update()
    {
        Quaternion targetRotation = rotatingToEnd ? endRotation : startRotation;
        // rotate between start and end rotation based on speed
        rotationProgress += rotationSpeed * Time.deltaTime;
        float rotationFraction = Mathf.Clamp01(rotationProgress / 180f);
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, rotationFraction);

        // if roation completes then reset it to change direction
        if (rotationFraction >= 1f)
        {
            rotatingToEnd = !rotatingToEnd;
            rotationProgress = 0f;
            if (rotatingToEnd)
            {
                Invoke(nameof(ResetRotationDelay), delay);
            }
        }
    }

    void ResetRotationDelay()
    {
        // Delay handled by Invoke
    }
}
