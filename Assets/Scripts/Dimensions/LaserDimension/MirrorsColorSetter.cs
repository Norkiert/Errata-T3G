using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class MirrorsColorSetter : MonoBehaviour
{
    private enum OnType { None, OnEnable, OnDsiable }

    [SerializeField] private List<GameObject> objectsToSetColor;
    [SerializeField] private Material materialToSet;
    [SerializeField] private bool setDefaltMaterialOnDisable = false;
    [SerializeField, ShowIf(nameof(setDefaltMaterialOnDisable))] private Material defaultMaterial;
    [SerializeField] private OnType disableInteractions = OnType.None;

    private void OnEnable()
    {
        foreach(GameObject mirror in objectsToSetColor)
        {
            if (mirror != null)
            {
                mirror.GetComponent<MeshRenderer>().material = materialToSet;

                if (disableInteractions == OnType.OnEnable)
                    mirror.GetComponent<Interactable>().enabled = false;
                else if (disableInteractions == OnType.OnDsiable)
                    mirror.GetComponent<Interactable>().enabled = true;
            }
        }
    }

    private void OnDisable()
    {
        foreach (GameObject mirror in objectsToSetColor)
        {
            if (mirror != null)
            {
                if (setDefaltMaterialOnDisable)
                    mirror.GetComponent<MeshRenderer>().material = defaultMaterial;

                if (disableInteractions == OnType.OnDsiable)
                    mirror.GetComponent<Interactable>().enabled = false;
                else if (disableInteractions == OnType.OnEnable)
                    mirror.GetComponent<Interactable>().enabled = true;
            }
        }
    }
}
