using UnityEngine;
using UnityEngine.Events;

public enum RaceState
{
    Preparation,
    CountDown,
    Race,
    Passed
}

public class RaceStateTracker : MonoBehaviour, IDependency<TrackPointCircuit>
{
    public event UnityAction PreparationStarted; // подготовка к старту
    public event UnityAction Started;
    public event UnityAction Completed;
    public event UnityAction<TrackPoint> TrackPointPassed;
    public event UnityAction<int> LapCompleted;

    private TrackPointCircuit trackPointCircuit;
    public void Construct(TrackPointCircuit trackPointCircuit) => this.trackPointCircuit = trackPointCircuit;

    [SerializeField] private Timer countDownTimer;
    public Timer CountDownTimer => countDownTimer;

    [SerializeField] private int lapsToComplete;

    //public bool isLastCircle = false;

    private RaceState state;
    public RaceState State => state;

    private void StartState(RaceState state)
    {
        this.state = state;
    }

    //public void Construct(TrackPointCircuit trackPointCircuit)
    //{
    //    this._trackPointCircuit = trackPointCircuit;
    //}

    private void Start()
    {
        StartState(RaceState.Preparation);

        //    if (_lapsToComplete == 1)
        //    {
        //        isLastCircle = true;
        //    }

        countDownTimer.enabled = false;

        countDownTimer.Finished += OnCountDownTimerFinished;

        trackPointCircuit.TrackPointTriggered += OnTrackPointTriggered;
        trackPointCircuit.LapCompleted += OnLapCompleted;
    }

    private void OnDestroy()
    {
        countDownTimer.Finished -= OnCountDownTimerFinished;

        trackPointCircuit.TrackPointTriggered -= OnTrackPointTriggered;
        trackPointCircuit.LapCompleted -= OnLapCompleted;
    }

    private void OnTrackPointTriggered(TrackPoint trackPoint)
    {
        TrackPointPassed?.Invoke(trackPoint);
    }

    private void OnCountDownTimerFinished()
    {
        StartRace();
    }

    private void OnLapCompleted(int lapAmount)
    {
        if (trackPointCircuit.Type == TrackType.Sprint)
        {
            CompleteRace();
        }

        if (trackPointCircuit.Type == TrackType.Circular)
        {
            if (lapAmount == lapsToComplete)
            {
                CompleteRace();
            }
            else
            {
                CompleteLap(lapAmount);
            }

            //if ((lapAmount + 1) == lapsToComplete)
            //{
            //    isLastCircle = true;
            //}
        }

    }

    public void LaunchPeparationStart()
    {
        if (state != RaceState.Preparation)
        {
            return;
        }

        StartState(RaceState.CountDown);

        countDownTimer.enabled = true;

        PreparationStarted?.Invoke();
    }

    private void StartRace()
    {
        if (state != RaceState.CountDown)
        {
            return;
        }

        StartState(RaceState.Race);

        Started?.Invoke();
    }

    private void CompleteRace()
    {
        if (state != RaceState.Race)
        {
            return;
        }

        StartState(RaceState.Passed);

        Completed?.Invoke();
    }

    private void CompleteLap(int lapAmount)
    {
        LapCompleted?.Invoke(lapAmount);
    }
}
