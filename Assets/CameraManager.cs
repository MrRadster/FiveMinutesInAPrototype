using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    public Camera officeCamera;
    public Camera[] securityCameras;

    [Header("UI")]
    public GameObject cameraUI;
    public RawImage cameraStaticOverlay; // Drag your UI RawImage here for static

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cameraToggleSFX;  
    [SerializeField] private AudioClip cameraSwitchSFX;  
    [SerializeField] private AudioClip staticHumSFX;      // Continuous camera static hum

    [Header("Red (Stare Animatronic)")]
    public Animatronic redAnimatronic;

    [Header("Which camera can see Red at each stage")]
    public int[] redVisibleOnCamera;

    private int currentCamIndex = -1;
    private bool isOnCameras = false;
    private float staticTimer = 0f;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

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

        // Animate the static texture UV coordinates for flickering noise movement
        if (isOnCameras && cameraStaticOverlay != null)
        {
            Rect currentUV = cameraStaticOverlay.uvRect;
            currentUV.x += Random.Range(-0.1f, 0.1f);
            currentUV.y += Random.Range(-0.1f, 0.1f);
            cameraStaticOverlay.uvRect = currentUV;
        }

        UpdateRedWatchStatus();
    }

    public void OpenCameras()
    {
        isOnCameras = true;
        officeCamera.enabled = false;
        cameraUI.SetActive(true);

        if (cameraStaticOverlay != null)
            cameraStaticOverlay.gameObject.SetActive(true);

        PlaySound(cameraToggleSFX);

        SwitchCamera(0);
    }

    public void CloseCameras()
    {
        if (isOnCameras)
            PlaySound(cameraToggleSFX);

        isOnCameras = false;
        cameraUI.SetActive(false);

        if (cameraStaticOverlay != null)
            cameraStaticOverlay.gameObject.SetActive(false);

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

        if (currentCamIndex != index)
        {
            PlaySound(cameraSwitchSFX);
        }

        if (currentCamIndex >= 0)
        {
            securityCameras[currentCamIndex].enabled = false;
        }

        currentCamIndex = index;
        securityCameras[currentCamIndex].enabled = true;

        UpdateRedWatchStatus();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void UpdateRedWatchStatus()
    {
        if (redAnimatronic == null) return;

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
        }

        redAnimatronic.SetBeingWatched(canSeeRed);
    }

    public bool IsOnCameras()
    {
        return isOnCameras;
    }
}