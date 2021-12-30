using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Interactable : MonoBehaviour
{
    [field: SerializeField] [field: ReadOnly] public bool IsSelected { get; private set; }

    protected virtual void Awake()
    {
        Select(false);
    }

    public void Select(bool state)
    {
        IsSelected = state;
        OnSelected(state);
    }

    protected virtual void OnSelected(bool state) { }
}
