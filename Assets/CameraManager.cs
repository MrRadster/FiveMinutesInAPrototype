using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    public Camera officeCamera;
    public Camera[] securityCameras;

    [Header("UI")]
    public GameObject cameraUI;

    [Header("Red (Stare Animatronic)")]
    public Animatronic redAnimatronic;

    [Header("Which camera can see Red at each stage")]
    public int[] redVisibleOnCamera;

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

        UpdateRedWatchStatus();
    }

    public void OpenCameras()
    {
        isOnCameras = true;
        officeCamera.enabled = false;
        cameraUI.SetActive(true);

        SwitchCamera(0);
    }

    public void CloseCameras()
    {
        isOnCameras = false;
        cameraUI.SetActive(false);

        foreach (Camera cam in securityCameras)
        {
            cam.enabled = false;
        }

        officeCamera.enabled = true;
        currentCamIndex = -1;

        if (redAnimatronic != null)
            redAnimatronic.SetBeingWatched(false);
    }

    public void SwitchCamera(int index)
    {
        if (!isOnCameras) return;
        if (index < 0 || index >= securityCameras.Length) return;

        if (currentCamIndex >= 0)
        {
            securityCameras[currentCamIndex].enabled = false;
        }

        currentCamIndex = index;
        securityCameras[currentCamIndex].enabled = true;

        UpdateRedWatchStatus();
    }

    void UpdateRedWatchStatus()
    {
        if (redAnimatronic == null)
            return;

        if (!isOnCameras)
        {
            redAnimatronic.SetBeingWatched(false);
            return;
        }

        int redStage = redAnimatronic.GetCurrentStage();
        bool canSeeRed = false;

        if (redVisibleOnCamera != null && redStage >= 0 && redStage < redVisibleOnCamera.Length)
        {
            int requiredCamera = redVisibleOnCamera[redStage];

            if (currentCamIndex == requiredCamera)
            {
                canSeeRed = true;
            }

            Debug.Log($"Red Stage: {redStage} | Current Cam: {currentCamIndex} | Needs Cam: {requiredCamera} | Watching: {canSeeRed}");
        }
        else
        {
            Debug.LogWarning($"Red stage {redStage} is outside the Red Visible On Camera array!");
        }

        redAnimatronic.SetBeingWatched(canSeeRed);
    }

    public bool IsOnCameras()
    {
        return isOnCameras;
    }
}