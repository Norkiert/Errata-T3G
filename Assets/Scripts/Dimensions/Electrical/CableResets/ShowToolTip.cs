using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowToolTip : Interactable
{
    [SerializeField] private GameObject canv;
    void Start()
    {
        canv.SetActive(false);
    }

    public override void Select()
    {
        base.Select();
        canv.SetActive(true);
        SetToolTip(true);
    }

    public override void Deselect()
    {
        base.Deselect();
        SetToolTip(false);
    }

    public void SetToolTip(bool value) => canv.SetActive(value);
}
