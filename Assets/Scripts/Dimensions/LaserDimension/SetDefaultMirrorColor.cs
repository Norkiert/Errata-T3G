using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDefaultMirrorColor : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToSetColor;
    [SerializeField] private Material materialToSet;
    void Start()
    {
        foreach (GameObject mirror in objectsToSetColor)
        {
            if (mirror != null)
                mirror.GetComponent<MeshRenderer>().material = materialToSet;
        }
    }

}
