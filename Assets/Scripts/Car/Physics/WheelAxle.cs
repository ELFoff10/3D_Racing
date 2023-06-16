using System;
using UnityEngine;

[Serializable]
/// <summary>
/// ������ ������� ���.
/// </summary>
public class WheelAxle
{
    [SerializeField] private WheelCollider leftWheelCollider;
    [SerializeField] private WheelCollider rightWheelCollider;

    [SerializeField] private Transform leftWheelMesh;
    [SerializeField] private Transform rightWheelMesh;

    [SerializeField] private bool isMotor; // ������ �����
    [SerializeField] private bool isSteer; // �������� �����

    [SerializeField] private float wheelWidth;

    [SerializeField] private float antiRollForce;

    [SerializeField] private float additionalWheelDownForce;

    [SerializeField] private float baseForwardStiffnes = 1.5f;
    [SerializeField] private float stabilityForwardFactor = 1.0f;

    [SerializeField] private float baseSdewaysStiffnes = 2.0f;
    [SerializeField] private float stabilitySidewaysFactor = 1.0f;

    private WheelHit leftWheelHit;
    private WheelHit rightWheelHit;

    public bool IsMotor => isMotor;
    public bool IsSteer => isSteer;

    // Public API
    public void Update()
    {
        UpdateWheelHits();
                
        ApplyAntiRoll();       
        ApplyDownForce();
        CorrectStiffness();

        SyncMeshTransform();
    }

    public void ConfigureVegicleSubsteps(float speedThreshold, int speedBelowThreshold, int stepsAboveThreshold)
    {
        leftWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveThreshold);
        rightWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveThreshold);
    }

    private void UpdateWheelHits()
    {
        leftWheelCollider.GetGroundHit(out leftWheelHit);
        rightWheelCollider.GetGroundHit(out rightWheelHit);
    }

    /// <summary>
    /// ����� ���� ������ ��� ����.
    /// </summary>
    private void CorrectStiffness()
    {
        WheelFrictionCurve leftForward = leftWheelCollider.forwardFriction;
        WheelFrictionCurve rightForward = rightWheelCollider.forwardFriction;

        WheelFrictionCurve leftSideways = leftWheelCollider.sidewaysFriction;
        WheelFrictionCurve rightSideways = rightWheelCollider.sidewaysFriction;

        leftForward.stiffness = baseForwardStiffnes + Mathf.Abs(leftWheelHit.forwardSlip) * stabilityForwardFactor;
        rightForward.stiffness = baseForwardStiffnes + Mathf.Abs(rightWheelHit.forwardSlip) * stabilityForwardFactor;

        leftSideways.stiffness = baseSdewaysStiffnes + Mathf.Abs(leftWheelHit.sidewaysSlip) * stabilitySidewaysFactor;
        rightSideways.stiffness = baseSdewaysStiffnes  + Mathf.Abs(rightWheelHit.sidewaysSlip) * stabilitySidewaysFactor;

        leftWheelCollider.forwardFriction = leftForward;
        rightWheelCollider.forwardFriction = rightForward;

        leftWheelCollider.sidewaysFriction = leftSideways;
        rightWheelCollider.sidewaysFriction = rightSideways;
    }

    /// <summary>
    /// ��������� ���� ��� ����.
    /// </summary>
    private void ApplyDownForce()
    {
        if (leftWheelCollider.isGrounded == true)
        {
            leftWheelCollider.attachedRigidbody.AddForceAtPosition(leftWheelHit.normal * -additionalWheelDownForce * 
                leftWheelCollider.attachedRigidbody.velocity.magnitude, leftWheelCollider.transform.position);
        }

        if (rightWheelCollider.isGrounded == true)
        {
            rightWheelCollider.attachedRigidbody.AddForceAtPosition(rightWheelHit.normal * -additionalWheelDownForce *
                rightWheelCollider.attachedRigidbody.velocity.magnitude, rightWheelCollider.transform.position);
        }
    }

    /// <summary>
    /// ������������ ���������� ������������.
    /// </summary>
    private void ApplyAntiRoll()
    {
        float travelL = 1.0f;
        float travelR = 1.0f;

        if (leftWheelCollider.isGrounded == true)
        {
            travelL = (-leftWheelCollider.transform.InverseTransformPoint(leftWheelHit.point).y - 
                leftWheelCollider.radius) / leftWheelCollider.suspensionDistance;
        }

        if (rightWheelCollider.isGrounded == true)
        {
            travelR = (-rightWheelCollider.transform.InverseTransformPoint(rightWheelHit.point).y - 
                rightWheelCollider.radius) / rightWheelCollider.suspensionDistance;
        }

        float forceDir = (travelL - travelR);

        if (leftWheelCollider.isGrounded == true) 
        {
            leftWheelCollider.attachedRigidbody.AddForceAtPosition(leftWheelCollider.transform.up * 
                -forceDir * antiRollForce, leftWheelCollider.transform.position);
        }

        if (rightWheelCollider.isGrounded == true)
        {
            rightWheelCollider.attachedRigidbody.AddForceAtPosition(rightWheelCollider.transform.up * 
                forceDir * antiRollForce, rightWheelCollider.transform.position);
        }
    }

    /// <summary>
    /// ��������� ���� �������� ������ � ������� ������, ���� � ��� isSteer = true.
    /// </summary>
    /// <param name="steerAngle"></param>
    public void ApplySteerAngle(float steerAngle, float wheelBaseLength)
    {
        if (isSteer == false) return;

        //TODO: ���� ��������
        float radius = Mathf.Abs(wheelBaseLength * Mathf.Tan(Mathf.Deg2Rad * (90 - Mathf.Abs(steerAngle))));        
        float angleSign = Mathf.Sign(steerAngle);

        //Debug.Log(steerAngle);
        //Debug.Log(angleSign);

        if (steerAngle > 0)
        {
            // ����� "������� ��� �� 7 ������ ������������ � ��������
            leftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (radius + (wheelWidth * 0.5f))) * angleSign;
            rightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (radius - (wheelWidth * 0.5f))) * angleSign;
        }
        else if (steerAngle < 0)
        {
            leftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (radius - (wheelWidth * 0.5f))) * angleSign;
            rightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (radius + (wheelWidth * 0.5f))) * angleSign;
        }
        else
        {
            leftWheelCollider.steerAngle = 0;
            rightWheelCollider.steerAngle = 0;
        }
    }

    /// <summary>
    /// ��������� ���� ��������� ������� ��������� ��� ������ � ������� ������, ���� � ��� isMotor = true.
    /// </summary>
    /// <param name="motorTorque"></param>
    public void ApplyMotorTorque(float motorTorque)
    {
        if (isMotor == false) return;

        leftWheelCollider.motorTorque = motorTorque; // ����� ������� ��� � ���������� motorTorque / 2
        rightWheelCollider.motorTorque = motorTorque;
    }

    /// <summary>
    /// ��������� ���� ����������
    /// </summary>
    /// <param name="brakeTorque"></param>
    public void ApplyBrakeTorque(float brakeTorque)
    {
        leftWheelCollider.brakeTorque = brakeTorque;
        rightWheelCollider.brakeTorque = brakeTorque;
    }

    public float GetAvarageRpm()
    {
        return (leftWheelCollider.rpm + rightWheelCollider.rpm) * 0.5f;
    }

    public float GetRadius()
    {
        return leftWheelCollider.radius;
    }

    // Private API

    /// <summary>
    /// ������������� ���������� � ����������� ����� � ������ ����.
    /// </summary>
    private void SyncMeshTransform()
    {
        UpdateWheelTransform(leftWheelCollider, leftWheelMesh);
        UpdateWheelTransform(rightWheelCollider, rightWheelMesh);
    }

    /// <summary>
    /// ����� � ����� ��������� � ��������� ������ � ��� ����� ����������.
    /// </summary>
    /// <param name="wheelCollider"></param>
    /// <param name="wheelTransform"></param>
    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        // ���������������� ���������� ���������� � ����
        wheelCollider.GetWorldPose(out position, out rotation);

        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}
