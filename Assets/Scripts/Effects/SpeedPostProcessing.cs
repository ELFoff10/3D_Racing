using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class SpeedPostProcessing : MonoBehaviour
{
    [SerializeField][Range(0.0f, 1.0f)] private float normalizeSpeed;
    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField] private Car car;

    private void Update()
    {
        float speed = car.NormalizeLinearVelocity;

        if (car.NormalizeLinearVelocity >= normalizeSpeed)
        {
            postProcessVolume.weight = speed;
        }
    }
}