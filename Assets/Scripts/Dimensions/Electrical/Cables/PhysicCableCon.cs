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
    }


    private bool CanConnect(Connector secondConnector) => secondConnector != _connector && secondConnector.ConnectionType != _connector.ConnectionType && secondConnector.ConnectionColor == _connector.ConnectionColor;
}
