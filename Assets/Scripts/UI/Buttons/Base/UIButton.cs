using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected bool Interactable;
    //public bool Interactable;

    private bool focus = false;
    public bool Focuse => focus;

    public UnityEvent OnClick;

    public event UnityAction<UIButton> PointerEnter;
    public event UnityAction<UIButton> PointerExit;
    public event UnityAction<UIButton> PointerClick;

    public virtual void SetFocuse()
    {
        if (Interactable == false)
        {
            return;
        }

        focus = true;
    }

    public virtual void SetUnFocuse()
    {
        if (Interactable == false)
        {
            return;
        }

        focus = false;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (Interactable == false)
        {
            return;
        }

        PointerEnter?.Invoke(this);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (Interactable == false)
        {
            return;
        }

        PointerExit?.Invoke(this);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (Interactable == false)
        {
            return;
        }

        PointerClick?.Invoke(this);
        OnClick?.Invoke();
    }
}