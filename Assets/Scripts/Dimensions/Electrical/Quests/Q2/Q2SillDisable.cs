using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class Q2SillDisable : MonoBehaviour
{
    [Header("Connectors to check")]
    [SerializeField] private Connector con1;
    [SerializeField] private Connector con2;

    [Header("Other")]
    [SerializeField] private float checkDelay = 2f;
    [SerializeField] private BoxCollider sill;
    [SerializeField] private ParticleSystem sparks;

    void Start()
    {
        if (con1 == null || con2 == null)
        {
            con1 = GameObject.Find("Q2FinalConnector").GetComponent<Connector>();
            con2 = GameObject.Find("Q2FinalConnectorCapsule").GetComponent<Connector>();
        }
        checkConnections = CheckConnections();
        StartCoroutine(checkConnections);
    }

    private IEnumerator checkConnections;

    private IEnumerator CheckConnections()
    {
        if(con1==null||con2==null)
        {
            con1 = GameObject.Find("Q2FinalConnector").GetComponent<Connector>();
            con2 = GameObject.Find("Q2FinalConnectorCapsule").GetComponent<Connector>();
        }
        while (!(con1.IsConnectedRight && con2.IsConnectedRight))
        {
            if (con1 == null || con2 == null)
            {
                con1 = GameObject.Find("Q2FinalConnector").GetComponent<Connector>();
                con2 = GameObject.Find("Q2FinalConnectorCapsule").GetComponent<Connector>();
            }
            yield return new WaitForSeconds(checkDelay);
        }
        TurnOffSill();
    }
    private IEnumerator checkDisconection;
    private IEnumerator CheckDisconection()
    {
        if (con1 == null || con2 == null)
        {
            con1 = GameObject.Find("Q2FinalConnector").GetComponent<Connector>();
            con2 = GameObject.Find("Q2FinalConnectorCapsule").GetComponent<Connector>();
        }
        while (con1.IsConnectedRight && con2.IsConnectedRight)
        {
            if (con1 == null || con2 == null)
            {
                con1 = GameObject.Find("Q2FinalConnector").GetComponent<Connector>();
                con2 = GameObject.Find("Q2FinalConnectorCapsule").GetComponent<Connector>();
            }
            yield return new WaitForSeconds(checkDelay);
        }
        TurnOnSill();
    }
    private void TurnOffSill()
    {
        if (checkConnections != null)
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

