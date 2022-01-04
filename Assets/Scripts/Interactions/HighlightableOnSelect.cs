using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class HighlightableOnSelect : Highlightable
{
    private Interactable interactable;
    protected new void Awake()
    {
        base.Awake();
        interactable = GetComponent<Interactable>();
    }

    protected new void Update()
    {
        if(isEnabled != interactable.IsSelected)
        {
            isEnabled = interactable.IsSelected;
            base.UpdateOnce();
        }
    }
}
