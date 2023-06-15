using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRaceResultPanel : MonoSingleton<UIRaceResultPanel>, IDependency<RaceResultTime>
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI recordTime;
    [SerializeField] private TextMeshProUGUI currentTime;

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