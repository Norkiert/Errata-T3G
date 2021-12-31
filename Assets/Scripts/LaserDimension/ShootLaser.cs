using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaser : Interactable
{
    [SerializeField] private Material material;
    [SerializeField] private float maxLaserLength=100f;
    LaserBeam beam;
    

    void Start()
    {
        Destroy(GameObject.Find("Laser Beam"));
        beam = new LaserBeam(gameObject.transform.position, gameObject.transform.right, material, maxLaserLength);
    }
    private void Update()
    {
        beam.LaserUpdate(gameObject.transform.position, gameObject.transform.right);
        if (IsSelected && (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)))
        {
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("E");
                transform.Rotate(0f, 0.2f, 0f);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                Debug.Log("Q");
                transform.Rotate(0f, -0.2f, 0f);
            }
        }
    }
}
