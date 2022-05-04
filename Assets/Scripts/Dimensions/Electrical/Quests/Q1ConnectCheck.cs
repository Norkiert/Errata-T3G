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
        while(!(con1.IsConnectedRight && con2.IsConnectedRight && con3.IsConnectedRight && con4.IsConnectedRight))
        {
            yield return new WaitForSeconds(checkDelay);
        }
        TurnOffSill();
    }
    private IEnumerator checkDisconection;
    private IEnumerator CheckDisconection()
    {
        while (con1.IsConnectedRight && con2.IsConnectedRight && con3.IsConnectedRight && con4.IsConnectedRight)
        {
            yield return new WaitForSeconds(checkDelay);
        }
        TurnOnSill();
    }
    private void TurnOffSill()
    {
        if(checkConnections!=null)
            StopCoroutine(checkConnections);
        sill.enabled = false;
        sparks.Stop();
        checkDisconection = CheckDisconection();
        StartCoroutine(checkDisconection);
    }
    private void TurnOnSill()
    {
        if (checkDisconection != null)
            StopCoroutine(checkDisconection);
        checkConnections = CheckConnections();
        StartCoroutine(checkConnections);
        sill.enabled = true;
        sparks.Play();
    }
}
