using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class Q1ConnectCheck : MonoBehaviour
{
    [Header("Connectors to check")]
    [SerializeField] private Connector con1;
    [SerializeField] private Connector con2;
    [SerializeField] private Connector con3;
    [SerializeField] private Connector con4;

    [Header ("Other")]
    [SerializeField] private float checkDelay = 2f;
    [SerializeField] private BoxCollider sill;
    [SerializeField] private ParticleSystem sparks;

    void Start()
    {
        checkConnections = CheckConnections();
        StartCoroutine(checkConnections);
    }

    private IEnumerator checkConnections;

    private IEnumerator CheckConnections()
    {
        while(!(con1.IsConnected && con2.IsConnected && con3.IsConnected && con4.IsConnected))
        {
            yield return new WaitForSeconds(checkDelay);
        }
        TurnOffSill();
    }
    private void TurnOffSill()
    {
        StopCoroutine(checkConnections);
        sill.enabled = false;
        sparks.Stop();
    }
}
