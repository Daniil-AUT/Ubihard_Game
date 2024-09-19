using UnityEngine;

public class CameraHorizontalRotation : MonoBehaviour
{
    public float rotationSpeed = 1;

    private Quaternion startRotation;
    private Quaternion endRotation;
    private bool rotatingToEnd = true;
    private float rotationProgress = 0f;

    void Start()
    {
        // canera will rotate horizontally 30 degrees to the right and then to left (loops)
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 30, 0));
    }

    // Needed some assistance with the physics of the camera so I used this link
    // https://vionixstudio.com/2022/06/16/unity-quaternion-and-rotation-guide/
    void Update()
    {
        Quaternion fromRotation = rotatingToEnd ? startRotation : endRotation;
        Quaternion toRotation = rotatingToEnd ? endRotation : startRotation;

        rotationProgress += rotationSpeed * Time.deltaTime / 200f;
        transform.rotation = Quaternion.Slerp(fromRotation, toRotation, rotationProgress);

        if (rotationProgress >= 1f)
        {
            rotatingToEnd = !rotatingToEnd;
            rotationProgress = 0f;
        }
    }
}
