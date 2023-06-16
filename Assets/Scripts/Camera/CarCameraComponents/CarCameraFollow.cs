using UnityEngine;

public class CarCameraFollow : CarCameraComponent
{
    [Header("OffSet")]
    [SerializeField] private float viewHeight;
    [SerializeField] private float height;
    [SerializeField] private float distance;

    [Header("Damping")]
    [SerializeField] private float rotationDamping;
    [SerializeField] private float heightDamping;
    [SerializeField] private float speedThreshold = 5;

    private Transform target;
    private new Rigidbody rigidbody;

    private void FixedUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 velocity = rigidbody.velocity;
        Vector3 targetRotation = target.eulerAngles;

        if (velocity.magnitude > speedThreshold)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up).eulerAngles;
        }

        float currentAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation.y, rotationDamping * Time.fixedDeltaTime);
        float currentHeight = Mathf.Lerp(transform.position.y, target.position.y + height, heightDamping * Time.fixedDeltaTime);

        Vector3 positionOffSet = Quaternion.Euler(0f, currentAngle, 0f) * Vector3.forward * distance;
        transform.position = target.position - positionOffSet;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        transform.LookAt(target.position + new Vector3(0, viewHeight, 0));
    }

    public override void SetProperties(Car car, Camera camera)
    {
        base.SetProperties(car, camera);

        target = car.transform;

        rigidbody = car.Rigidbody;
    }
}
