using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repelling : MonoBehaviour
{
    [SerializeField] private PointThunder pointThunder;
    [SerializeField] private Vector3 pushDirection;
    [SerializeField] private float pushForce;
    [SerializeField] private CharacterController characterController;

    private Vector3 impact = Vector3.zero;

    private void Strike(Vector3 pos)
    {
        pointThunder.SpawnThunder(pos);
    }

    private void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        impact += dir * force;
    }

    private void Update()
    {
        if (impact.magnitude > 0.2) characterController.Move(impact * Time.deltaTime);
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject player = other.gameObject;
        if (player.GetComponent<PlayerController>() != null)
        {
            Strike(transform.position);
            AddImpact(pushDirection, pushForce);
        }
    }
}
