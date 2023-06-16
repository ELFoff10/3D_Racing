using UnityEngine;

public class SpeedEffect : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 1.0f)] private float normalizeSpeed;
    [SerializeField] private ParticleSystem speedLine;

    [SerializeField] private Car car;

    private void Update()
    {
        if (car.NormalizeLinearVelocity >= normalizeSpeed)
        {
            speedLine.Emit(1);
        }
        else
        {
            speedLine.Stop();
        }
    }
}
