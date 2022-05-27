using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;
using UnityEngine.UI;

public class OnHoverOnClick : MonoBehaviour
{
    public AudioClipSO audioOnHover;
    public AudioClipSO audioOnClick;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void onClick()
    {
        if (button.interactable)
            AudioManager.PlaySFX(audioOnClick);
    }

    public void onHover()
    {
        if (button.interactable)
            AudioManager.PlaySFX(audioOnHover);
    }
}
