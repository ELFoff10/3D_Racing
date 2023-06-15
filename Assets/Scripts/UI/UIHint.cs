using UnityEngine;

public class UIHint : MonoBehaviour
{
    [SerializeField] private GameObject hint;

    private void Start()
    {
        hint.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            hint.SetActive(false);
        }
    }
}
