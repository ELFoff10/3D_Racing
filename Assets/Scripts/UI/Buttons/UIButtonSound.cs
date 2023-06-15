using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip select;

    private AudioSource audioSource;

    private UIButton[] uiButton;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        uiButton = GetComponentsInChildren<UIButton>(true);

        for (int i = 0; i < uiButton.Length; i++)
        {
            uiButton[i].PointerEnter += OnPointerEnter;
            uiButton[i].PointerClick += OnPointerClicked;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < uiButton.Length; i++)
        {
            uiButton[i].PointerEnter -= OnPointerEnter;
            uiButton[i].PointerClick -= OnPointerClicked;
        }
    }

    private void OnPointerEnter(UIButton uIButton)
    {
        audioSource.PlayOneShot(select);
    }

    private void OnPointerClicked(UIButton uIButton)
    {
        audioSource.PlayOneShot(click);
    }
}