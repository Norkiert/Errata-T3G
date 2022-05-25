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
        canv.SetActive(true);
        base.Select();
    }

    public override void Deselect()
    {
        canv.SetActive(false);
        base.Deselect();
    }
}
