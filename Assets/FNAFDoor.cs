using UnityEngine;

public class FNAFDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public float openSpeed = 4f;
    public float openHeight = 2.6f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorCloseSFX;
    [SerializeField] private AudioClip doorOpenSFX;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen = true;        
    private bool isMoving = false;

    void Start()
    {
        openPos = transform.position;
        closedPos = openPos - Vector3.up * openHeight;

        // Automatically grab the AudioSource if not assigned in Inspector
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!isMoving) return;

        Vector3 target = isOpen ? openPos : closedPos;
        transform.position = Vector3.MoveTowards(transform.position, target, openSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            isMoving = false;
        }
    }

    public void ToggleDoor()
    {
        if (isMoving) return;

        isOpen = !isOpen;
        isMoving = true;

        // Play the appropriate sound effect when movement begins
        if (isOpen)
        {
            PlaySound(doorOpenSFX);
        }
        else
        {
            PlaySound(doorCloseSFX);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}