using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управление автомобилем.
/// </summary>
public class CarInputControl : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private AnimationCurve brakeCurve;
    [SerializeField] private AnimationCurve steerCurve;


    [SerializeField][Range(0.0f, 1.0f)] private float autoBrakeStrength = 0.5f;

    private float wheelSpeed;
    private float verticalAxis;
    private float horizontalAxis;
    //private float handBrakeAxis;

    private void Update()
    {
        wheelSpeed = car.WheelSpeed;

        UpdateAxis();

        UpdateThrottleAndBrake();
        UpdateSteer();

        UpdateAutoBrake();

        //DEBUG
        if (Input.GetKeyDown(KeyCode.E))
        {
            car.UpGear();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            car.DownGear();
        }
    }
    private void UpdateThrottleAndBrake() 
    {
        if (Mathf.Sign(verticalAxis) == Mathf.Sign(wheelSpeed) || Mathf.Abs(wheelSpeed) < 0.5f)
        {
            car.ThrottleControl = Mathf.Abs(verticalAxis);
            car.BrakeControl = 0;
        }
        else
        {
            car.ThrottleControl = 0;
            car.BrakeControl = brakeCurve.Evaluate(wheelSpeed / car.MaxSpeed);
        }

        //Gears
        if (verticalAxis < 0 && wheelSpeed > -0.5f && wheelSpeed <= 0.5f)
        {
            car.ShiftToReverseGear();
        }

        if (verticalAxis > 0 && wheelSpeed > -0.5f && wheelSpeed < 0.5f)
        {
            car.ShiftToFirstGear();
        }
    }

    private void UpdateSteer()
    {
        car.SteerControl = steerCurve.Evaluate(wheelSpeed / car.MaxSpeed) * horizontalAxis;
    }

    private void UpdateAutoBrake()
    {
        if (verticalAxis == 0)
        {
            car.BrakeControl = brakeCurve.Evaluate(wheelSpeed / car.MaxSpeed) * autoBrakeStrength;
        }
    }

    private void UpdateAxis()
    {
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");
        //handBrakeAxis = Input.GetAxis("Jump"); // Самостоятельно в видео 29.6 на 19 минут говорится.
    }

    public void Reset()
    {
        verticalAxis = 0;
        horizontalAxis = 0;
        //handBrakeAxis = 0;

        car.ThrottleControl = 0;
        car.SteerControl = 0;
        car.BrakeControl = 0;
        //car.HandBrakeAxis = 0;
    }

    public void Stop()
    {
        Reset();

        car.BrakeControl = 0.5f;
    }
}
