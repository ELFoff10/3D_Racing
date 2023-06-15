using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UISelectableButton : UIButton
{
    [SerializeField] private Image selectImage;

    public UnityEvent OnSelect;
    public UnityEvent OnUnSelect;

    public override void SetFocuse()
    {
        base.SetFocuse();

        if (selectImage != null)
        {
            selectImage.enabled = true;
        }

        OnSelect?.Invoke();
    }

    public override void SetUnFocuse()
    {
        base.SetUnFocuse();

        if (selectImage != null)
        {
            selectImage.enabled = false;
        }

        OnUnSelect?.Invoke();
    }
}