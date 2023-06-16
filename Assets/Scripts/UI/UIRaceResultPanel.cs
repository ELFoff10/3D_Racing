using TMPro;
using UnityEngine;

public class UIRaceResultPanel : MonoSingleton<UIRaceResultPanel>, IDependency<RaceResultTime>
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text recordTime;
    [SerializeField] private TMP_Text currentTime;

    private RaceResultTime raceResultTime;
    public void Construct(RaceResultTime obj) => raceResultTime = obj;

    private void Start()
    {
        resultPanel.SetActive(false);

        raceResultTime.ResultUpdated += OnUpdateResult;
    }

    private void OnDestroy()
    {
        raceResultTime.ResultUpdated -= OnUpdateResult;
    }

    private void OnUpdateResult()
    {
        resultPanel.SetActive(true);

        recordTime.text = StringTime.SecondToTimeString(raceResultTime.GetAbsoluteRecord());
        currentTime.text = StringTime.SecondToTimeString(raceResultTime.CurrentTime);
    }
}