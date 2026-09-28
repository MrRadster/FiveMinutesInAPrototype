using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    public Camera officeCamera;
    public Camera[] securityCameras;

    [Header("UI")]
    public GameObject cameraUI;

    private int currentCamIndex = -1;
    private bool isOnCameras = false;

    void Start()
    {
        CloseCameras();
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.C))
    {
        if (isOnCameras)
            CloseCameras();
        else
            OpenCameras();
    }
}
    public void OpenCameras()
    {
        isOnCameras = true;
        officeCamera.enabled = false;
        cameraUI.SetActive(true);

        // Open the first camera by default
        SwitchCamera(0);
    }

    public void CloseCameras()
    {
        isOnCameras = false;
        cameraUI.SetActive(false);

        // Disable all security cameras
        foreach (Camera cam in securityCameras)
        {
            cam.enabled = false;
        }

        officeCamera.enabled = true;
        currentCamIndex = -1;
    }

    public void SwitchCamera(int index)
    {
        if (!isOnCameras) return;
        if (index < 0 || index >= securityCameras.Length) return;

        // Disable previous camera
        if (currentCamIndex >= 0)
        {
            securityCameras[currentCamIndex].enabled = false;
        }

        // Enable new camera
        currentCamIndex = index;
        securityCameras[currentCamIndex].enabled = true;
    }
}

