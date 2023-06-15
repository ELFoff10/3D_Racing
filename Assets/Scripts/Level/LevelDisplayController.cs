using UnityEngine;

public class LevelDisplayController : MonoBehaviour
{
    [SerializeField] private UIRaceButton[] raceButton;
    
    private void Start()
    {
        var drawLevel = 0;

        var score = 1;

        while (score != 0 && drawLevel < raceButton.Length &&
                LevelCompletion.Instance.TryIndex(drawLevel, out var race, out score))
        {
            raceButton[drawLevel].SetLevelData(race, score);
            drawLevel += 1;
        }

        for (int i = drawLevel; i < raceButton.Length; i++)
        {
            Debug.Log("r231");
            //_levels[i].gameObject.SetActive(false);
            raceButton[i].LockPanel.SetActive(false);
            raceButton[i].GetComponent<UIRaceButton>().enabled = true;

        }

        #region Last
        /*        while (score != 0 && drawLevel < _levels.Length)
                {           
                    score = _levels[drawLevel].Initialise();
                    drawLevel += 1;
                }

                for (int i = drawLevel; i < _levels.Length; i++)
                {
                    //_levels[i].gameObject.SetActive(false);
                    _levels[i].LockPanel.SetActive(true);
                    _levels[i].GetComponent<UIRaceButton>().enabled = false;
                }*/
        #endregion
    }
}