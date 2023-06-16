using UnityEngine;

public class UIPausePanel : MonoBehaviour, IDependency<Pauser>
{
    [SerializeField] private GameObject panel;

    private Pauser pauser;
    public void Construct(Pauser obj) => pauser = obj;

    private void Start()
    {
        panel.SetActive(false);
        pauser.PauseStateChange += OnPauseStateChange;
    }

    private void OnDestroy()
    {
        pauser.PauseStateChange -= OnPauseStateChange;
    }

    private void OnPauseStateChange(bool isPause)
    {
        panel.SetActive(isPause);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            pauser.ChangePauseState();
        }
    }

    public void UnPause()
    {
        pauser.UnPause();
    }
}
