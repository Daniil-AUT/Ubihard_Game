using UnityEngine;

public class CameraHorizontalRotation : MonoBehaviour
{
    public float rotationSpeed = 30f; // Speed of the rotation (degrees per second)
    public float delay = 2f; // Delay before rotating back

    private Quaternion startRotation; // Starting rotation
    private Quaternion endRotation; // Ending rotation (180 degrees from start)
    private bool rotatingToEnd = true; // Whether the camera is rotating towards the end position
    private float rotationProgress = 0f; // Progress of the rotation

    void Start()
    {
        // Store the initial rotation
        startRotation = transform.rotation;

        // Calculate the end rotation (180 degrees horizontal from start)
        endRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 45, 0));
    }

    void Update()
    {
        // Determine the target rotation
        Quaternion targetRotation = rotatingToEnd ? endRotation : startRotation;

        // Smoothly rotate towards the target rotation
        rotationProgress += rotationSpeed * Time.deltaTime;
        float rotationFraction = Mathf.Clamp01(rotationProgress / 180f); // Ensure fraction is between 0 and 1
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, rotationFraction);

        // Check if the rotation is complete
        if (rotationFraction >= 1f)
        {
            // Switch rotation direction and reset progress
            rotatingToEnd = !rotatingToEnd;
            rotationProgress = 0f;

            // Add a delay before rotating back
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
