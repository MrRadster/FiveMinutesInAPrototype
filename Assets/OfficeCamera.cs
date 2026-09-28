using UnityEngine;

public class OfficeCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public float panSpeed = 40f;
    public float maxPanAngle = 45f;
    public float edgeThreshold = 0.1f;

    private float currentPan = 0f;
    private Quaternion startRotation;

    void Start()
    {
        // Remember the camera's original rotation
        startRotation = transform.localRotation;
    }

    void Update()
    {
        float mouseX = Input.mousePosition.x / Screen.width;

        // Look left
        if (mouseX < edgeThreshold)
        {
            currentPan -= panSpeed * Time.deltaTime;
        }
        // Look right
        else if (mouseX > 1f - edgeThreshold)
        {
            currentPan += panSpeed * Time.deltaTime;
        }

        // Clamp the pan amount
        currentPan = Mathf.Clamp(currentPan, -maxPanAngle, maxPanAngle);

        // Apply pan on top of the original rotation
        transform.localRotation = startRotation * Quaternion.Euler(0f, currentPan, 0f);
    }
}