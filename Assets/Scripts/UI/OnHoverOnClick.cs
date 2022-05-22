using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

public class OnHoverOnClick : MonoBehaviour
{
    public AudioClipSO audioOnHover;
    public AudioClipSO audioOnClick;

    public void onClick()
    {
        AudioManager.PlaySFX(audioOnClick);
    }

    public void onHover()
    {
        AudioManager.PlaySFX(audioOnHover);
    }
}
