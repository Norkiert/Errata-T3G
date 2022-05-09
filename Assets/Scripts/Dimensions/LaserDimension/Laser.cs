using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logic;
using NaughtyAttributes;

[RequireComponent(typeof(LineRenderer))]
public class Laser : LogicBoolOutput
{
    [SerializeField, Range(0f, 1f)] private float laserSize = 0.1f;
    [SerializeField, Min(1)] private float maxLaserLength = 100f;
    [SerializeField, ReadOnly] private bool targetHit = false;
    [SerializeField] private LayerMask layerMask = 1;

    private LineRenderer laser;
    private readonly List<Vector3> laserIndices = new List<Vector3>();

    public override bool LogicValue => targetHit;

    private void Start()
    {
        laser = gameObject.GetComponent<LineRenderer>();
        laser.startWidth = laserSize;
        laser.endWidth = laserSize;
    }

    private void Update()
    {
        LaserUpdate();
    }

    private void LaserUpdate()
    {
        targetHit = false;
        laserIndices.Clear();
        CastRay(transform.position, transform.right);
        UpdateLaserBeam();
    }

    private void CastRay(Vector3 position, Vector3 direction)
    {
        laserIndices.Add(position);
        Ray ray = new Ray(position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, maxLaserLength, layerMask))
        {
            CheckHit(hit, direction);
        }
        else
        {
            laserIndices.Add(ray.GetPoint(maxLaserLength));
        }
    }

    private void CheckHit(RaycastHit hitInfo, Vector3 direction)
    {
        if (hitInfo.collider.gameObject.TryGetComponent(out LaserMirror _))
        {
            Vector3 hitPosition = hitInfo.point - direction * laserSize * 0.1f;
            Debug.DrawLine(hitPosition, hitInfo.point, Color.blue);
            Vector3 newDirection = Vector3.Reflect(direction, hitInfo.normal);
            CastRay(hitPosition, newDirection);
            return;
        }

        if (hitInfo.collider.gameObject.TryGetComponent(out LaserTarget _))
        {
            targetHit = true;
        }
        
        laserIndices.Add(hitInfo.point);
    }

    private void UpdateLaserBeam()
    {
        int length = laserIndices.Count;
        laser.positionCount = length;

        for (int i = 0; i < length; i++)
            laser.SetPosition(i, laserIndices[i]);
    }
    private void OnDisable()
    {
        targetHit = false;
    }
}
