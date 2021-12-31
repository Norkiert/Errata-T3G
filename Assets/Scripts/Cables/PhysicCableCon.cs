using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Connector))]
public class PhysicCableCon : Liftable
{
    private Connector connector;
    protected override void Awake()
    {
        base.Awake();

        connector = gameObject.GetComponent<Connector>();
    }

    public override void Drop()
    {
        base.Drop();
        
        Interactable selecredObject = FindObjectOfType<PlayerInteractions>().SelectedObject;
        if (selecredObject && selecredObject.TryGetComponent(out Connector secondConnector) && secondConnector != connector)
        {
            secondConnector.Connect(connector);
        }
    }
    public override void PickUp(int layer)
    {
        base.PickUp(layer);

        if (connector.ConnectedTo)
            connector.Disconnect();
    }
}
