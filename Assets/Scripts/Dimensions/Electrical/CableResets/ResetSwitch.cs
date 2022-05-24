using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResetSwitch : Clickable
{
    [SerializeField] private CableReset reseter;

    protected override void Awake()
    {
        base.Awake();
        OnClick += Rotate;
    }

    private void Rotate() => StartCoroutine(RotateSwitch());

    private IEnumerator RotateSwitch()
    {
        reseter.RespawnCables();
        transform.DOLocalRotate(new Vector3(0, 0, 180), 0.5f);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(Time.deltaTime);
        transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
    }

}
