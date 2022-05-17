using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class Clickable : Interactable
{
    public event Action OnClick;

    public void Click()
    {
        OnClick?.Invoke();
    }
}
