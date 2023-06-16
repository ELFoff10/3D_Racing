using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRaceButton : UISelectableButton/*, IScriptableObjectProperty*/
{
    [SerializeField] private RaceInfo raceInfo;
    [SerializeField] private GameObject lockPanel;

    public GameObject LockPanel => lockPanel;

    private void Start()
    {
        ApplyProperty(raceInfo);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (raceInfo == null)
        {
            return;
        }

        SceneManager.LoadScene(raceInfo.SceneName);
    }

    public void ApplyProperty(RaceInfo property)
    {
        if (property == null)
        {
            return;
        }

        raceInfo = property;

        //if (property is raceinfo == false)
        //{
        //    return;
        //}
    }
    public void SetLevelData(RaceInfo race, int score)
    {
        raceInfo = race;
    }
}
