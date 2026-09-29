using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [Header("Ambience Audio")]
    [SerializeField] private AudioSource ambienceSource;
    [SerializeField] private AudioClip officeAmbience;

    void Start()
    {
        if (ambienceSource == null)
            ambienceSource = GetComponent<AudioSource>();

        if (ambienceSource != null && officeAmbience != null)
        {
            ambienceSource.clip = officeAmbience;
            ambienceSource.loop = true;
            ambienceSource.playOnAwake = true;
            ambienceSource.Play();
        }
    }
}