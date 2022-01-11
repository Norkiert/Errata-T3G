using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChrisNolet;

public class Highlightable : Outline
{
    [SerializeField] protected bool isEnabled = false;

    protected new void Awake()
    {
        if (isEnabled)
        {
            base.firstTime = false;
        }
        base.Awake();
        needsUpdate = false;
        base.UpdateMaterialProperties();
        if (!isEnabled)
        {
            base.Disable();
        }
    }

    protected new void OnValidate()
    {
        UpdateOnce();
    }

    protected void UpdateOnce()
    {
        if (isEnabled)
        {
            base.Enable();
        }
        else
        {
            base.Disable();
        }
        base.OnValidate();
    }
}
