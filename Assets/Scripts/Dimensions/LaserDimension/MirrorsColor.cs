using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorsColor : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToSetColor;
    [SerializeField] private Material materialToSet;
    [SerializeField] private Material defaultMaterial;
    void OnEnable()
    {
        foreach(GameObject mirror in objectsToSetColor)
        {
            if(mirror!=null)
                mirror.GetComponent<MeshRenderer>().material = materialToSet;
        }
    }

    void OnDisable()
    {
        foreach (GameObject mirror in objectsToSetColor)
        {
            if(mirror!=null)
                mirror.GetComponent<MeshRenderer>().material = defaultMaterial;
        }
    }

}
