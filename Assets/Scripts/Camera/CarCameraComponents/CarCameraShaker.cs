using UnityEngine;

public class CarCameraShaker : CarCameraComponent
{
    [SerializeField][Range(0f, 1f)] private float normalizeSpeedSnake;
    [SerializeField] private float shakeAmount;

    private void Update()
    {
        ShakeTheCamera();
    }

    private void ShakeTheCamera()
    {
        if (car.NormalizeLinearVelocity >= normalizeSpeedSnake)
        {
            transform.localPosition += Random.insideUnitSphere * shakeAmount * Time.deltaTime;
        }
    }
}
