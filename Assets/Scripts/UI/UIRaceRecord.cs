using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRaceRecord : MonoBehaviour, IDependency<RaceResultTime>, IDependency<RaceStateTracker>
{
    [SerializeField] private GameObject goldRecordObject;
    [SerializeField] private GameObject playerRecordObject;
    [SerializeField] private TextMeshProUGUI goldRecordTime;
    [SerializeField] private TextMeshProUGUI playerRecordTime;

    private RaceResultTime raceResultTime;
    public void Construct(RaceResultTime obj) => raceResultTime = obj;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private void Start()
    {
        raceStateTracker.Started += OnRaceStart;
        raceStateTracker.Completed += OnRaceCompleted;

        goldRecordObject.SetActive(false);
        playerRecordObject.SetActive(false);
    }

    private void OnDestroy()
    {
        raceStateTracker.Started -= OnRaceStart;
        raceStateTracker.Completed -= OnRaceCompleted;
    }

    private void OnRaceStart()
    {
        if (raceResultTime.PlayerRecordTime > raceResultTime.GoldTime || raceResultTime.IsRecordWasSet == false)
        {
            goldRecordObject.SetActive(true);
            goldRecordTime.text = StringTime.SecondToTimeString(raceResultTime.GoldTime);
        }

        if (raceResultTime.IsRecordWasSet == true)
        {
            playerRecordObject.SetActive(true);
            playerRecordTime.text = StringTime.SecondToTimeString(raceResultTime.PlayerRecordTime);
        }
    }

    private void OnRaceCompleted()
    {
        goldRecordObject.SetActive(false);
        playerRecordObject.SetActive(false);
    }
}