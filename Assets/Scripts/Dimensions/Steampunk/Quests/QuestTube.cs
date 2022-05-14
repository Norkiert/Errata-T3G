using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTube : MonoBehaviour
{
    [SerializeField] List<MeshRenderer> tubeRenderers;

    [SerializeField] Material materialNotCompleted;
    [SerializeField] Material materialCompleted;

    [SerializeField] LayerMask ballLayer;

    protected bool triggered = false;

    protected void OnTriggerEnter(Collider other)
    {
        if(!triggered && (1 << other.gameObject.layer & ballLayer.value) != 0)
        {
            triggered = true;
            foreach(var tubeRenderer in tubeRenderers)
            {
                tubeRenderer.material = materialCompleted;
            }
        }
    }
}
