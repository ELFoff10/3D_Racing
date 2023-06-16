using UnityEngine;
using UnityEngine.Events;

enum GearName
{
    R, N
}

[RequireComponent (typeof(CarChassis))]
/// <summary>
/// Информационная модель автомобиля.
/// </summary>

// Основная идея, что мы CarChassis выступает посредником, для получения данных из WheelAxle
public class Car : MonoBehaviour // Все скрипты будут взаимодействовать с классом Car    
{
    public event UnityAction<string> GearChanged;

    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float maxBreakTorque;

    [Header("Engine")]
    [SerializeField] private AnimationCurve engineTorqueCurve;
    [SerializeField] private float engineMaxTorque;
    // DEBUG
    [SerializeField] private float engineTorque;
    [SerializeField] private float engineRpm;

    [SerializeField] private float engineMinRpm;
    [SerializeField] private float engineMaxRpm;

    [Header("Gearbox")]
    [SerializeField] private float[] gears;
    [SerializeField] private float finalDriveRatio;

    //DEBUG
    [SerializeField] private int selectedGearIndex;
    //DEBUG
    [SerializeField] private float selectedGear;
    [SerializeField] private float rearGear;
    [SerializeField] private float upShiftEngineRpm;
    [SerializeField] private float downShiftEngineRpm;

    [SerializeField] private int maxSpeed;
    public float MaxSpeed => maxSpeed;
    public float LinearVelocity => chassis.LinearVelocity;
    public float NormalizeLinearVelocity => chassis.LinearVelocity / maxSpeed;
    public float WheelSpeed => chassis.GetWheelSpeed();

    public float EngineRpm => engineRpm;
    public float EngineMaxRpm => engineMaxRpm;

    private CarChassis chassis;
    public Rigidbody Rigidbody => chassis == null ? GetComponent<CarChassis>().Rigidbody : chassis.Rigidbody;

    // DEBUG
    public float ThrottleControl; //  Педаль газа. "Дроссель"
    public float SteerControl; // Поворот
    public float BrakeControl; // Тормоз
    //public float handbrakecontrol; // ручной тормоз

    private void Start()
    {
        chassis = GetComponent<CarChassis>();
    }

    private void Update()
    {
        UpdateEngineTorque();

        AutoGearShift();

        if (LinearVelocity >= maxSpeed) 
        {
            engineTorque = 0;
        }

        chassis.MotorTorque = engineTorque * ThrottleControl;
        chassis.SteerAngle = maxSteerAngle * SteerControl;
        chassis.BreakTorque = maxBreakTorque * BrakeControl;
    }

    // Gearbox 

    public string GetSelectedGearName()
    {
        if (selectedGear == rearGear)
        {
            return GearName.R.ToString();
        }

        if (selectedGear == 0)
        {
            return GearName.N.ToString();
        }

        return (selectedGearIndex + 1).ToString();
    }

    private void AutoGearShift()
    {
        if (selectedGear < 0) return;

        if (engineRpm >= upShiftEngineRpm)
        {
            UpGear();
        }
        if (engineRpm <= downShiftEngineRpm)
        {
            DownGear();
        }
    }

    public void UpGear()
    {
        ShiftGear(selectedGearIndex + 1);
    }
    public void DownGear()
    {
        ShiftGear(selectedGearIndex - 1);
    }
    public void ShiftToReverseGear()
    {
        selectedGear = rearGear;
        GearChanged?.Invoke(GetSelectedGearName());
    }
    public void ShiftToFirstGear()
    {
        ShiftGear(0);
    }
    public void ShiftToNeutralGear()
    {
        selectedGear = 0;
        GearChanged?.Invoke(GetSelectedGearName());
    }

    private void ShiftGear(int gearIndex)
    {
        gearIndex = Mathf.Clamp(gearIndex, 0, gears.Length - 1);
        selectedGear = gears[gearIndex];
        selectedGearIndex = gearIndex;

        GearChanged?.Invoke(GetSelectedGearName());
    }

    private void UpdateEngineTorque()
    {
        engineRpm = engineMinRpm + Mathf.Abs(chassis.GetAverageRpm() * selectedGear * finalDriveRatio);
        engineRpm = Mathf.Clamp(engineRpm, engineMinRpm, engineMaxRpm);

        engineTorque = engineTorqueCurve.Evaluate(engineRpm / engineMaxRpm) * engineMaxTorque * finalDriveRatio * Mathf.Sign(selectedGear) * gears[0];
    }

    public void Reset()
    {
        chassis.Reset();

        chassis.MotorTorque = 0;
        chassis.BreakTorque = 0;
        chassis.SteerAngle = 0;

        ThrottleControl = 0;
        BrakeControl = 0;
        SteerControl = 0;
    }

    public void Respawn(Vector3 position, Quaternion rotation)
    {
        Reset();
        transform.position = position;
        transform.rotation = rotation;
    }
}
