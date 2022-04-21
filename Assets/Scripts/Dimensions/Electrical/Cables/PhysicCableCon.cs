using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Connector))]
public class PhysicCableCon : Liftable
{
    private Connector _connector;

    protected override void Awake()
    {
        base.Awake();

        _connector = gameObject.GetComponent<Connector>();
    }

    public override void Drop()
    {
        base.Drop();
        
        Interactable selecredObject = FindObjectOfType<PlayerInteractions>().SelectedObject;
        if (selecredObject && selecredObject.TryGetComponent(out Connector secondConnector) && CanConnect(secondConnector))
        {
            secondConnector.Connect(_connector);
        }
    }
    public override void PickUp(int layer)
    {
        base.PickUp(layer);

        if (_connector.ConnectedTo)
            _connector.Disconnect();



        StartCoroutine(UpdatePositionOfPreviouesCable());
    }

    private IEnumerator UpdatePositionOfPreviouesCable()
    {
        PhysicCable cable = GetComponentInParent<PhysicCable>();
        Transform previousPoint = null;
        if (_connector == cable.StartConnector)
            previousPoint = cable.Points[1];
        else if (_connector == cable.EndConnector)
            previousPoint = cable.Points[cable.Points.Count - 2];

        if (previousPoint == null)
        {
            Debug.LogWarning($"{name}: Cant find previous point!");
            yield break;
        }

        Rigidbody thisRB = GetComponent<Rigidbody>();
        Rigidbody previousRB = previousPoint.GetComponent<Rigidbody>();
        previousRB.isKinematic = true;

        SpringJoint spring = null;
        foreach (var s in previousPoint.GetComponents<SpringJoint>())
            if (s.connectedBody == thisRB)
                spring = s;
        spring.connectedBody = null;

        while (IsLift)
        {
            if (Vector3.Distance(transform.position, previousPoint.position) > 0.1)
                previousPoint.position += (transform.position - previousPoint.position) * Mathf.Clamp01(Time.deltaTime * 20);
            yield return null;
        }

        previousRB.isKinematic = false;
        spring.connectedBody = thisRB;
    }

    private bool CanConnect(Connector secondConnector) => secondConnector != _connector && secondConnector.ConnectionType != _connector.ConnectionType && secondConnector.ConnectionColor == _connector.ConnectionColor;
}
