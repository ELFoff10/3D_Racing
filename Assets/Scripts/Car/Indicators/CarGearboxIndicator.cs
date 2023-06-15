using TMPro;
using UnityEngine;

public class CarGearboxIndicator : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private TextMeshProUGUI text;

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
