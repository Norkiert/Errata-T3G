using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResetSwitch : Interactable
{
    [SerializeField] private CableReset reseter;

    public override void Select()
    {
        base.Select();
        StartCoroutine(checkPress());
    }
    public override void Deselect()
    {
        base.Deselect();
        transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
        StopAllCoroutines();
    }

    IEnumerator checkPress()
    {
        while (IsSelected)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(RotateSwitch());
            }
            yield return null;
        }
    }

    private IEnumerator RotateSwitch()
    {
        reseter.RespawnCables();
        transform.DOLocalRotate(new Vector3(0, 0, 180), 0.5f);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(Time.deltaTime);
        transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
    }

}
