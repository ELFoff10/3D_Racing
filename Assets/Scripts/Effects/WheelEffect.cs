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
        bool isSlip = false; // изначально автомобиль не проскальзывает

        for (int i = 0; i < wheels.Length; i++) //1. узнаём проскыльзывают ли колёса
        {
            wheels[i].GetGroundHit(out wheelHit);

            if (wheels[i].isGrounded == true) // если колесо на земле
            {
                if (Mathf.Abs(wheelHit.forwardSlip) > forwardSlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) > sidewaySlipLimit)/*if (wheelHit.forwardSlip > forwardSlipLimit || wheelHit.sidewaysSlip > sidewaySlipLimit)*/ // если колесо скользит вперед больше чем лимит или боковое скольжение больше чем лимит это означае что колесо сейчас проскальзывает
                {
                    if (skidTrail[i] == null) // есть ли у нас в массиве этот траил
                        skidTrail[i] = Instantiate(skidPrefab).transform; // и если нет то мы его создаём

                    if (audio.isPlaying == false) //если звук не играет, тогда воспроизводим его
                        audio.Play();

                    if (skidTrail[i] != null) // если есть скидтраил
                    {
                        skidTrail[i].position = wheels[i].transform.position - wheelHit.normal * wheels[i].radius * 1.75f; // задаём позицию равную как transform.position(центр колеса) - wheelHit.normal(направление колеса) * wheels[i].radius
                        skidTrail[i].forward = -wheelHit.normal; // поворачиваем скидтраил по нормали

                        wheelsSmoke[i].transform.position = skidTrail[i].position;
                        wheelsSmoke[i].Emit(1); // запускаем систему частиц Emit
                    }

                    isSlip = true; // проскальзывают колёса

                    continue; // для перехода к следующему колесу
                }
            }

            skidTrail[i] = null; // обнуляем зн-я (как только мы оторвались от земли или перестали скользить)
            wheelsSmoke[i].Stop(); // останавливаем дым
        }

        if (isSlip == false) // если колёса не проскальзывают тогда останавливаем музыку(в цикл выше мы не зашли)
            audio.Stop();
    }
}
