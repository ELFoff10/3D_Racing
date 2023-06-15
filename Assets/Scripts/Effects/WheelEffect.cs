using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WheelEffect : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheels;
    [SerializeField] private ParticleSystem[] wheelsSmoke;

    [SerializeField] private float forwardSlipLimit;
    [SerializeField] private float sidewaySlipLimit;

    [SerializeField] private new AudioSource audio;

    [SerializeField] private GameObject skidPrefab;

    private WheelHit wheelHit;
    private Transform[] skidTrail;

    private void Start()
    {
        skidTrail = new Transform[wheels.Length];
    }

    private void Update()
    {
        bool isSlip = false; // ���������� ���������� �� ��������������

        for (int i = 0; i < wheels.Length; i++) //1. ����� �������������� �� �����
        {
            wheels[i].GetGroundHit(out wheelHit);

            if (wheels[i].isGrounded == true) // ���� ������ �� �����
            {
                if (Mathf.Abs(wheelHit.forwardSlip) > forwardSlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) > sidewaySlipLimit)/*if (wheelHit.forwardSlip > forwardSlipLimit || wheelHit.sidewaysSlip > sidewaySlipLimit)*/ // ���� ������ �������� ������ ������ ��� ����� ��� ������� ���������� ������ ��� ����� ��� ������� ��� ������ ������ ��������������
                {
                    if (skidTrail[i] == null) // ���� �� � ��� � ������� ���� �����
                        skidTrail[i] = Instantiate(skidPrefab).transform; // � ���� ��� �� �� ��� ������

                    if (audio.isPlaying == false) //���� ���� �� ������, ����� ������������� ���
                        audio.Play();

                    if (skidTrail[i] != null) // ���� ���� ���������
                    {
                        skidTrail[i].position = wheels[i].transform.position - wheelHit.normal * wheels[i].radius * 1.75f; // ����� ������� ������ ��� transform.position(����� ������) - wheelHit.normal(����������� ������) * wheels[i].radius
                        skidTrail[i].forward = -wheelHit.normal; // ������������ ��������� �� �������

                        wheelsSmoke[i].transform.position = skidTrail[i].position;
                        wheelsSmoke[i].Emit(1); // ��������� ������� ������ Emit
                    }

                    isSlip = true; // �������������� �����

                    continue; // ��� �������� � ���������� ������
                }
            }

            skidTrail[i] = null; // �������� ��-� (��� ������ �� ���������� �� ����� ��� ��������� ���������)
            wheelsSmoke[i].Stop(); // ������������� ���
        }

        if (isSlip == false) // ���� ����� �� �������������� ����� ������������� ������(� ���� ���� �� �� �����)
            audio.Stop();
    }
}
