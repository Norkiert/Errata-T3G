using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChrisNolet;

public class Highlightable : Outline
{
    [SerializeField] protected bool isEnabled = false;

    protected new void Awake()
    {
        triggerEnable = false;

        base.Awake();
        needsUpdate = false;
        base.UpdateMaterialProperties();

        if (isEnabled)
            base.Enable();
        else
            base.Disable();
    }

    protected new void OnValidate()
    {
        UpdateOnce();
    }

    protected void UpdateOnce()
    {
        if (isEnabled)
            base.Enable();
        else
            base.Disable();

        base.OnValidate();
    }

    private new void OnEnable()
    {
        if (isEnabled)
            base.Enable();
    }

    private new void OnDisable()
    {
        base.Disable();
    }
}
