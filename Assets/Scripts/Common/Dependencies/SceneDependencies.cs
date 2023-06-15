using UnityEngine;

public interface IDependency<T>
{
    void Construct(T obj);
}

public class SceneDependencies : Dependencies
{
    [SerializeField] private RaceStateTracker raceStateTracker;
    [SerializeField] private RaceTimeTracker raceTimeTracker;
    [SerializeField] private RaceResultTime raceResultTime;
    [SerializeField] private CarInputControl carInputControl;
    [SerializeField] private TrackPointCircuit trackPointCircuit;
    [SerializeField] private Car car;
    [SerializeField] private CarCameraController carCameraController;

    protected override void BindAll(MonoBehaviour monoBehaviorInScene)
    {
        Bind<RaceStateTracker>(raceStateTracker, monoBehaviorInScene);
        Bind<CarInputControl>(carInputControl, monoBehaviorInScene);
        Bind<TrackPointCircuit>(trackPointCircuit, monoBehaviorInScene);
        Bind<Car>(car, monoBehaviorInScene);
        Bind<CarCameraController>(carCameraController, monoBehaviorInScene);
        Bind<RaceTimeTracker>(raceTimeTracker, monoBehaviorInScene);
        Bind<RaceResultTime>(raceResultTime, monoBehaviorInScene);
    }

    private void Awake()
    {
        FindAllObjectToBind();
    }

    //private void Awake()
    //{
    //    MonoBehaviour[] allMonoBehavioursInScene = FindObjectsOfType<MonoBehaviour>();

    //    for (int i = 0; i < allMonoBehavioursInScene.Length; i++)
    //    {
    //        Bind(allMonoBehavioursInScene[i]);
    //    }
    //}

    //private void Bind(MonoBehaviour mono)
    //{
    //    if (mono is IDependency<RaceStateTracker>)
    //    {
    //        (mono as IDependency<RaceStateTracker>).Construct(raceStateTracker);
    //    }

    //    if (mono is IDependency<CarInputControl>)
    //    {
    //        (mono as IDependency<CarInputControl>).Construct(carInputControl);
    //    }

    //    if (mono is IDependency<TrackPointCircuit>)
    //    {
    //        (mono as IDependency<TrackPointCircuit>).Construct(trackPointCircuit);
    //    }

    //    if (mono is IDependency<Car>)
    //    {
    //        (mono as IDependency<Car>).Construct(car);
    //    }

    //    if (mono is IDependency<CarCameraController>)
    //    {
    //        (mono as IDependency<CarCameraController>).Construct(carCameraController);
    //    }

    //    if (mono is IDependency<RaceTimeTracker>)
    //    {
    //        (mono as IDependency<RaceTimeTracker>).Construct(raceTimeTracker);
    //    }

    //    if (mono is IDependency<RaceResultTime>)
    //    {
    //        (mono as IDependency<RaceResultTime>).Construct(raceResultTime);
    //    }
    //}

    #region 3 way setting
    //1 Вариант
    /*private void Bind(MonoBehaviour mono)
    {
        if (mono is IDependency<RaceStateTracker>)
        {
            (mono as IDependency<RaceStateTracker>).Construct(raceStateTracker);
        }

        if (mono is IDependency<CarInputControl>)
        {
            (mono as IDependency<CarInputControl>).Construct(carInputControl);
        }

        if (mono is IDependency<TrackPointCircuit>)
        {
            (mono as IDependency<TrackPointCircuit>).Construct(trackPointCircuit);
        }

        if (mono is IDependency<Car>)
        {
            (mono as IDependency<Car>).Construct(car);
        }

        if (mono is IDependency<CarCameraController>)
        {
            (mono as IDependency<CarCameraController>).Construct(carCameraController);
        }

        if (mono is IDependency<RaceTimeTracker>)
        {
            (mono as IDependency<RaceTimeTracker>).Construct(raceTimeTracker);
        }

        if (mono is IDependency<RaceResultTime>)
        {
            (mono as IDependency<RaceResultTime>).Construct(raceResultTime);
        }
    }*/

    //2 Вариант
    /*    private void Bind(MonoBehaviour mono)
        {
            IDependency<TrackPointCircuit> t = mono as IDependency<TrackPointCircuit>;

            if (t != null)
            {
                t.Construct(_trackPointCircuit);
            }
        }*/

    //3 Вариант 
    /*    private void Bind(MonoBehaviour mono)
        {
            (mono as IDependency<TrackPointCircuit>)?.Construct(_trackPointCircuit);
        }*/

    #endregion
}