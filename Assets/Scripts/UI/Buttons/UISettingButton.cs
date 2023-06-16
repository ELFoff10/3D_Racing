using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISettingButton : UISelectableButton/*, IScriptableObjectProperty*/
{
    [SerializeField] private Settings setting;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Image previousImage;
    [SerializeField] private Image nextImage;

    private void Start()
    {
        ApplyProperty(setting);
    }

    public void SetNextValueSetting()
    {
        setting?.SetNextValue();
        setting?.Apply();
        UpdateInfo();
    }

    public void SetPreviousValueSetting()
    {
        setting?.SetPreviousValue();
        setting?.Apply();
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        titleText.text = setting.Title;
        valueText.text = setting.GetStringValue();

        previousImage.enabled = !setting.isMinValue;
        nextImage.enabled = !setting.isMaxValue;
    }

    public void ApplyProperty(Settings property/*ScriptableObject setting*/)
    {
        //if (setting == null)
        //{
        //    return;
        //}

        //if (setting is Settings == false)
        //{
        //    return;
        //}

        //this.setting = setting as Settings;

        //UpdateInfo();

        if (property == null)
        {
            return;
        }
        setting = property;

        UpdateInfo();
    }
}