using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using Audio;

public class Q2Switch : Interactable
{
    [SerializeField, Required] private ParticleSystem switchParticles;
    [SerializeField, Required] private Connector con1;
    [SerializeField, Required] private Connector con2;
    [SerializeField, Required] private Connector con3;
    [SerializeField, Required] private Connector con4;
    [SerializeField, Required] private Connector con5;
    [SerializeField, Required] private Connector con6;
    [SerializeField, Required] private Q2Capsule capsuleConnector;
    [SerializeField] private AudioClipSO invalidSound;

    private bool done = false;
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
        if(con1.IsConnected&& con2.IsConnected && con3.IsConnected && con4.IsConnected && con5.IsConnected && con6.IsConnected && !done)
        {
            done = true;
            transform.DOLocalRotate(new Vector3(0, 0, 180), 1f);
            capsuleConnector.SLideCapsuleDown();
        }
        else
        {
            if(!done)
                StartCoroutine(RotateSwitch());
        }    
    }

    private IEnumerator RotateSwitch()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 30), 0.2f);
        yield return new WaitForSeconds(0.1f);
        AudioManager.PlaySFX(invalidSound, transform.position);
        switchParticles.Play();
        yield return new WaitForSeconds(Time.deltaTime);
        transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
    }


}
