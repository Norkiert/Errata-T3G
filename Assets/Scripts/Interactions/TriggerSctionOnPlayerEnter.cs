using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerSctionOnPlayerEnter : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEnter;

    [Header("Play only one time")]
    [SerializeField] private bool playOnlyOneTime = false;
    [SerializeField, ShowIf(nameof(playOnlyOneTime))] private string uniqueSaveKey = "undefined";

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerInteractions>() != null)
        {
            if (playOnlyOneTime)
            {
                if (HroberPrefs.ReadBool(uniqueSaveKey, false))
                    return;

                HroberPrefs.SaveBool(uniqueSaveKey, true);
            }

            OnEnter?.Invoke();
        }
    }
}
