using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderOpacity : MonoBehaviour
{
    private Material material;
    private Transform player;
    private float distance;
    private float radiusOfCylinder;
    private void Start()
    {
        material = GameObject.Find("TerrainDisappearCylinder").GetComponent<Renderer>().material;
        material.color = new Color(1, 1, 1, 0);
        radiusOfCylinder = GameObject.Find("TerrainDisappearCylinder").transform.localScale.x/2;
        player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        distance = Mathf.Sqrt(Mathf.Pow(player.position.x,2) + Mathf.Pow(player.position.z, 2));
        float actualOpacity = distance - radiusOfCylinder;
        if(actualOpacity>0)
        {
            material.color = new Color(1,1,1,actualOpacity/100);
        }
    }
}
