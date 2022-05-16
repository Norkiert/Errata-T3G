using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class PillarBehaviour : MonoBehaviour
{
    [SerializeField, ReadOnly] private Connector conToCheck;
    [SerializeField] private string obToFind;
    [SerializeField] private ParticleSystem partSys;
    [SerializeField] private Renderer pillar;

    [SerializeField] Material matConnected;
    [SerializeField] Material matDisconnected;

    [SerializeField] private float checkStep = 1f;
    void Start()
    {
        if (getConnectors != null)
            StopCoroutine(getConnectors);
        getConnectors = GetConnectors();
        StartCoroutine(getConnectors);
    }

    private IEnumerator checkConnect;

    private IEnumerator CheckConnect()
    {
        while (!conToCheck.IsConnectedRight)
            yield return new WaitForSeconds(checkStep);
        partSys.Stop();
        pillar.material = matDisconnected;
        if (checkDisconnect != null)
            StopCoroutine(checkDisconnect);
        checkDisconnect = CheckDisconnect();
        StartCoroutine(checkDisconnect);
    }
    private IEnumerator checkDisconnect;

    private IEnumerator CheckDisconnect()
    {
        while (conToCheck.IsConnectedRight)
            yield return new WaitForSeconds(checkStep);
        partSys.Play();
        pillar.material = matConnected;
        if (checkConnect != null)
            StopCoroutine(checkConnect);
        checkConnect = CheckConnect();
        StartCoroutine(checkConnect);
    }
    private IEnumerator getConnectors;
    private IEnumerator GetConnectors()
    {
        yield return new WaitForSeconds(0.1f);
        if(GameObject.Find(obToFind).TryGetComponent<Connector>(out _)&& conToCheck==null)
            conToCheck = GameObject.Find(obToFind).GetComponent<Connector>();
        if (checkConnect != null)
            StopCoroutine(checkConnect);
        checkConnect = CheckConnect();
        StartCoroutine(checkConnect);
    }
}
