using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RaceResultTime : MonoBehaviour, IDependency<RaceTimeTracker>, IDependency<RaceStateTracker>
{
    public const string SaveMark = "_player_best_time";

    public event UnityAction ResultUpdated;
    public event UnityAction ScoreUpdate;

    [SerializeField] private float goldTime;
    public float GoldTime => goldTime; 

    public bool IsRecordWasSet => playerRecordTime != 0;

    private float playerRecordTime;
    public float PlayerRecordTime => playerRecordTime;

    private float currentTime;
    public float CurrentTime => currentTime;

    private RaceTimeTracker raceTimeTracker;
    public void Construct(RaceTimeTracker obj) => raceTimeTracker = obj;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private bool isCompleted = false;
    public bool IsCompleted => isCompleted;

    private void Awake()
    {
        Load();
    }

    private void Start()
    {
        raceStateTracker.Completed += OnRaceCompleted;
    }

    private void OnDestroy()
    {
        raceStateTracker.Completed -= OnRaceCompleted;
    }

    private void OnRaceCompleted()
    {
        float absoluteRecord = GetAbsoluteRecord();

        if (raceTimeTracker.CurrentTime < absoluteRecord || playerRecordTime == 0)
        {
            playerRecordTime = raceTimeTracker.CurrentTime;

            Save();
        }

        currentTime = raceTimeTracker.CurrentTime;

        ResultUpdated?.Invoke();
    }

    public float GetAbsoluteRecord()
    {
        if (playerRecordTime < goldTime && playerRecordTime != 0)
        {
            ScoreUpdate?.Invoke();
            return playerRecordTime;
        }
        else
        {
            return goldTime;
        }
    }

    private void Load()
    {
        playerRecordTime = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + SaveMark, 0);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + SaveMark, playerRecordTime);
    }
}