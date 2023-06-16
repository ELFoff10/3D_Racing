using TMPro;
using UnityEngine;

public class CarGearboxIndicator : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        car.GearChanged += OnGearChanged;
    }

    private void OnDestroy()
    {
        car.GearChanged -= OnGearChanged;
    }

    private void OnGearChanged(string gearName)
    {
        text.text = gearName;
    }
}
