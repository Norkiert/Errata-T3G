using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

public class Repelling : MonoBehaviour
{
    [SerializeField] private ParticleSystem touchSpark;
    [SerializeField] private ParticleSystem sparks;
    [SerializeField] private AudioClipSO touchSound;
    [SerializeField] private Vector3 pushDirection;
    [SerializeField] private float pushForce;
    private CharacterController characterController;

    private Vector3 impact = Vector3.zero;

    private void Start()
    {
        characterController = GameObject.Find("Player").GetComponent<CharacterController>();
        sparks.Play();
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
            touchSpark.Play();
            AddImpact(pushDirection, pushForce);
            AudioManager.PlaySFX(touchSound, transform.position);
        }
    }
}
