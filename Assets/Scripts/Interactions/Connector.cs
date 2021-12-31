using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody))]
public class Connector : MonoBehaviour
{
    [SerializeField] private Transform connectionPoint;
    [field: SerializeField] [field: ReadOnly] public Connector ConnectedTo { get; private set; }

    private FixedJoint fixedJoint;
    public Rigidbody Rigidbody { get; private set; }

    public Vector3 ConnectionPosition => connectionPoint ? connectionPoint.position : transform.position;
    public Quaternion ConnectionRotation => connectionPoint ? connectionPoint.rotation : transform.rotation;

    private void Awake()
    {
        Rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void SetAsConnectedTo(Connector secondConnector) => ConnectedTo = secondConnector;

    public void Connect(Connector secondConnector)
    {
        if (secondConnector == null)
        {
            Debug.LogWarning("Attempt to connect null");
            return;
        }

        if (ConnectedTo != null)
            Disconnect();

        secondConnector.transform.rotation = ConnectionRotation;
        secondConnector.transform.position = ConnectionPosition - (secondConnector.ConnectionPosition - secondConnector.transform.position);

        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = secondConnector.Rigidbody;

        secondConnector.SetAsConnectedTo(this);
        ConnectedTo = secondConnector;
    }

    public void Disconnect(Connector onlyThis = null)
    {
        if (ConnectedTo == null || onlyThis != null && onlyThis != ConnectedTo)
            return;

        Destroy(fixedJoint);

        // important to dont make recusrion
        Connector toDisconect = ConnectedTo;
        ConnectedTo = null;
        toDisconect.Disconnect(this);
    }
}
