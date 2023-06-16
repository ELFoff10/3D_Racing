using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
/// <summary>
/// Физика всего автомобиля.
/// </summary>
public class CarChassis : MonoBehaviour
{
    [SerializeField] private WheelAxle[] wheelAxles;
    [SerializeField] private float wheelBaseLenght;

    [SerializeField] private Transform centerOfMass;

    [Header("Down Force")]
    [SerializeField] private float downForceMin;
    [SerializeField] private float downForceMax;
    [SerializeField] private float downForceFactor;

    [Header("AngularDrag")]
    [SerializeField] private float AngularDragMin;
    [SerializeField] private float AngularDragMax;
    [SerializeField] private float AngularDragFactor;

    public float MotorTorque;
    public float BreakTorque;
    public float SteerAngle;

    public float LinearVelocity => rigidbody.velocity.magnitude * 3.6f;

    private new Rigidbody rigidbody;
    public Rigidbody Rigidbody => rigidbody == null ? GetComponent<Rigidbody>() : rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (centerOfMass != null)
        {
            rigidbody.centerOfMass = centerOfMass.localPosition;
        }

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            wheelAxles[i].ConfigureVegicleSubsteps(50, 50, 50);
        }
    }

    private void FixedUpdate()
    {
        UpdateAngularDrag();

        UpdateDownForce();

        UpdateWheelAxles();
    }

    /// <summary>
    /// Среднее вращение всех колёс.
    /// </summary>
    /// <returns></returns>
    public float GetAverageRpm()
    {
        float sum = 0;

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            sum += wheelAxles[i].GetAvarageRpm();
        }

        return sum / wheelAxles.Length;
    }

    public float GetWheelSpeed()
    {
        return GetAverageRpm() * wheelAxles[0].GetRadius() * 2 * 0.1885f;
    }

    private void UpdateAngularDrag()
    {
        rigidbody.angularDrag = Mathf.Clamp(AngularDragFactor * LinearVelocity, AngularDragMin, AngularDragMax);
    }

    private void UpdateDownForce()
    {
        float downForce = Mathf.Clamp(downForceFactor * LinearVelocity, downForceMin, downForceMax);

        rigidbody.AddForce(-transform.up * downForce);
    }

    private void UpdateWheelAxles()
    {
        int amounMotorWheel = 0;

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            if (wheelAxles[i].IsMotor == true)
            {
                amounMotorWheel += 2;
            }
        }

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            wheelAxles[i].Update();

            wheelAxles[i].ApplyMotorTorque(MotorTorque / amounMotorWheel); 
            wheelAxles[i].ApplySteerAngle(SteerAngle, wheelBaseLenght);
            wheelAxles[i].ApplyBrakeTorque(BreakTorque);
        }
    }

    public void Reset()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}
