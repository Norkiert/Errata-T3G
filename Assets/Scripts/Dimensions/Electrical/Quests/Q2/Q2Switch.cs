using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Q2Switch : Interactable
{
    public override void Select()
    {
        base.Select();
        StartCoroutine(checkPress());
    }
    public override void Deselect()
    {
        base.Deselect();
        StopAllCoroutines();
    }

    IEnumerator checkPress()
    {
        while(IsSelected)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                TrySwitchSwitchDown();
            }
            yield return null;
        }
    }
    private void TrySwitchSwitchDown()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 180), 1f);
    }



}
