using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Q3FakeConnector : MonoBehaviour
{ 
[SerializeField] private Connector connectorToCheck;
[SerializeField] private GameObject[] objects;
[SerializeField] private Collider[] objectsWithCollider;
[SerializeField] private float duration = 1f;
[SerializeField] private float checkDelay = 1.5f;
void Start()
{
    if (connectorToCheck != null)
        DisconnectedCable();
}

private IEnumerator checkConnect;

private IEnumerator CheckConnect()
{
    yield return new WaitForSeconds(checkDelay);
    while (!connectorToCheck.IsConnectedRight)
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
    foreach (GameObject obj in objects)
    {
        obj.transform.DORotate(new Vector3(-85, obj.transform.eulerAngles.y, 0), duration);
    }
    foreach (Collider col in objectsWithCollider)
    {
        col.enabled = false;
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
        obj.transform.DORotate(new Vector3(0, obj.transform.eulerAngles.y, 0), duration);
    }
        foreach (Collider col in objectsWithCollider)
        {
            col.enabled = true;
        }
        checkConnect = CheckConnect();
    StartCoroutine(checkConnect);
}

}
