using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICountDownTimer : MonoBehaviour, IDependency<RaceStateTracker>
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Timer countDownTimer;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private void Start()
    {
        raceStateTracker.PreparationStarted += OnRacePreparationStarted;
        raceStateTracker.Started += OnRaceStarted;

        text.enabled = false;
    }

    private void OnDestroy()
    {
        raceStateTracker.PreparationStarted -= OnRacePreparationStarted;
        raceStateTracker.Started -= OnRaceStarted;
    }

    private void OnRacePreparationStarted()
    {
        text.enabled = true;
        enabled = true;
    }

    private void OnRaceStarted()
    {
        text.enabled = false;
        enabled = false;
    }

    private void Update()
    {
        text.text = raceStateTracker.CountDownTimer.Value.ToString("F0");

        if (text.text == "0")
        {
            text.text = "GO!";
        }
    }
}
