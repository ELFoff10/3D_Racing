using System;
using UnityEngine;
using UnityEngine.UI;

public class CarEngineIndicator : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private Image image;
    [SerializeField] private EngineIndicatorColor[] colors;

    private void Update()
    {
        ChangeImageEngineIndicator();
    }

    private void ChangeImageEngineIndicator()
    {
        image.fillAmount = car.EngineRpm / car.EngineMaxRpm;

        for (int i = 0; i < colors.Length; i++)
        {
            if (car.EngineRpm <= colors[i].MaxRpm)
            {
                image.color = colors[i].color;
                break;
            }
        }
    }

    [Serializable]
    struct EngineIndicatorColor
    {
        public float MaxRpm;
        public Color color;
    }
}
