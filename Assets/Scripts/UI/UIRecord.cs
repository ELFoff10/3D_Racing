using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRecord : MonoBehaviour, IDependency<RaceResultTime>, IDependency<RaceStateTracker>, IDependency<RaceTimeTracker>
{
    [SerializeField] private GameObject panelRaceRecordTime;
    [SerializeField] private GameObject panelBest;
    [SerializeField] private TextMeshProUGUI textRecordText;
    [SerializeField] private TextMeshProUGUI textCurrentText;

    private RaceResultTime raceResultTime;
    public void Construct(RaceResultTime obj) => raceResultTime = obj;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private RaceTimeTracker raceTimeTracker;
    public void Construct(RaceTimeTracker obj) => raceTimeTracker = obj;

    private void Start()
    {
        raceStateTracker.Completed += OnRaceCompleted;

        panelRaceRecordTime.SetActive(false);
    }

    private void OnDestroy()
    {
        raceStateTracker.Completed -= OnRaceCompleted;
    }

    private void OnRaceCompleted()
    {
        panelRaceRecordTime.SetActive(true);

        if (raceResultTime.PlayerRecordTime >= raceTimeTracker.CurrentTime)
        {       
            panelBest.SetActive(true);
        }

        else
        {
            panelBest.SetActive(false);
        }

        textRecordText.text = StringTime.SecondToTimeString(raceResultTime.PlayerRecordTime);
        textCurrentText.text = StringTime.SecondToTimeString(raceTimeTracker.CurrentTime);
    }
}