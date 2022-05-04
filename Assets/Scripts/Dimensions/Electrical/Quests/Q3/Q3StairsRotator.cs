using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Q3StairsRotator : MonoBehaviour
{
    [SerializeField] private Connector connectorToCheck;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private float flyDistance = 5f;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float checkDelay = 2f;
    void Start()
    {
        if (connectorToCheck != null)
            DisconnectedCable();
    }

    private IEnumerator checkConnect;

    private IEnumerator CheckConnect()
    {
        yield return new WaitForSeconds(checkDelay);
        while(!connectorToCheck.IsConnectedRight)
        {
            yield return new WaitForSeconds(checkDelay);
        }
        ConnectedCable();
    }
    private IEnumerator checkDisconnect;

    private IEnumerator CheckDisconnect()
    {
        yield return new WaitForSeconds(checkDelay);
        while (connectorToCheck.IsConnectedRight)
        {
            yield return new WaitForSeconds(checkDelay);
        }
        DisconnectedCable();
    }

    private void ConnectedCable()
    {
        if (checkConnect != null)
            StopCoroutine(checkConnect);
        foreach(GameObject obj in objects)
        {
            obj.transform.DOMoveY(obj.transform.position.y + flyDistance, duration);
        }
        checkDisconnect = CheckDisconnect();
        StartCoroutine(checkDisconnect);
    }
    private void DisconnectedCable()
    {
        if (checkDisconnect != null)
            StopCoroutine(checkDisconnect);
        foreach (GameObject obj in objects)
        {
            obj.transform.DOMoveY(obj.transform.position.y - flyDistance, duration);
        }
        checkConnect = CheckConnect();
        StartCoroutine(checkConnect);
    }

}
