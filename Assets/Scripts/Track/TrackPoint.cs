using UnityEngine;
using UnityEngine.Events;

public class TrackPoint : MonoBehaviour
{
    public event UnityAction<TrackPoint> Triggered;

    protected virtual void OnPassed() { }
    protected virtual void OnAssignTarget() { }

    public TrackPoint Next;
    public bool IsFirst;
    public bool IsLast;
    //public bool IsFinish = false;

    protected bool isTarget;
    public bool IsTarget => isTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponent<Car>() == null)
        {
            return;
        }

        Triggered?.Invoke(this);
    }

    public void Passed()
    {
        isTarget = false;
        //IsFinish = true;
        OnPassed();
    }


    /// <summary>
    /// Назначаем точку как таргет
    /// </summary>
    public void AssignAsTarget() 
    {
        isTarget = true;
        OnAssignTarget();
    }

    public void Reset()
    {
        Next = null;
        IsFirst = false;
        IsLast = false;
    }
}
