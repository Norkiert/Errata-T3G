using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Interactable : MonoBehaviour
{
    [field: SerializeField] [field: ReadOnly] public bool IsSelected { get; private set; }

    protected virtual void Awake()
    {
        Deselect();
    }

    public virtual void Select()
    {
        IsSelected = true;
    }

    public virtual void Deselect()
    {
        IsSelected = false;
    }
}