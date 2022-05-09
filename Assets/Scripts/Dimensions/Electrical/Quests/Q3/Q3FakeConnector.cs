using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Q3FakeConnector : MonoBehaviour
{
    [SerializeField] private Connector connectorToCheck;
    [SerializeField] private GameObject[] objects;
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
        yield return new WaitForSeconds(checkDelay/4);
        while (!connectorToCheck.IsConnectedRight)
        {
            yield return new WaitForSeconds(checkDelay/4);
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
        if (rotateFake != null)
            StopCoroutine(rotateFake);
        rotateFake = RotateFake();
        StartCoroutine(rotateFake);
        checkDisconnect = CheckDisconnect();
        StartCoroutine(checkDisconnect);
    }
    private void DisconnectedCable()
    {
        if (checkDisconnect != null)
            StopCoroutine(checkDisconnect);
        checkConnect = CheckConnect();
        StartCoroutine(checkConnect);
    }

    private IEnumerator rotateFake;

    private IEnumerator RotateFake()
    {
        foreach (GameObject obj in objects)
        {
            obj.transform.DORotate(new Vector3(-90, obj.transform.eulerAngles.y, 0), duration);
        }
        yield return new WaitForSeconds(duration);
        foreach (GameObject obj in objects)
        {
            obj.transform.DORotate(new Vector3(0, obj.transform.eulerAngles.y, 0), duration);
        }
    }
}
