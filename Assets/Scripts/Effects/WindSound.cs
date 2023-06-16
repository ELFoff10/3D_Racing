using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WindSound : MonoBehaviour
{
    //public event UnityAction<float> SpeedFast;

    [SerializeField] private Car car;

    [SerializeField][Range(0f, 1f)] private float normalizeCarSpeed = 0.2f;
    [SerializeField][Range(0f, 1f)] private float volumeReduceBeforeOff = 0.001f;
    [SerializeField][Range(0f, 1f)] private float minSizeVolumeValueBeforeOff = 0.1f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (car.NormalizeLinearVelocity >= normalizeCarSpeed)
        {
            audioSource.volume = 1;
            audioSource.enabled = true;
        }
        else
        {
            audioSource.volume -= volumeReduceBeforeOff;

            if (audioSource.volume <= minSizeVolumeValueBeforeOff)
            {
                audioSource.enabled = false;
            }
        }
    }
}
