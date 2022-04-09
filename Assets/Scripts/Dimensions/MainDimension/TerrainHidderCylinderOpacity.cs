using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TerrainHidderCylinderOpacity : MonoBehaviour
{
    [SerializeField, Required] private Transform terrainDisappearCylinder;

    private Material material;
    private Transform player;
    private float distance;
    private float radiusOfCylinder;

    private void Start()
    {
        material = terrainDisappearCylinder.GetComponent<Renderer>().material;
        material.color = new Color(1, 1, 1, 0);
        radiusOfCylinder = terrainDisappearCylinder.localScale.x / 2;

        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning($"{name}: dont found player!");
            enabled = false;
        }
        else
            player = playerController.transform;
    }
    private void Update()
    {
        distance = Mathf.Sqrt(Mathf.Pow(player.position.x,2) + Mathf.Pow(player.position.z, 2));
        float actualOpacity = distance - radiusOfCylinder;
        if(actualOpacity > 0)
        {
            material.color = new Color(1,1,1,actualOpacity / 100);
        }
    }
}
