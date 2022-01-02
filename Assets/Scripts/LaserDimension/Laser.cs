using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : Interactable
{
    [SerializeField] [Min(1)] private float maxLaserLength = 100f;

    private LineRenderer laser;
    private List<Vector3> laserIndices = new List<Vector3>();

    private void Start()
    {
        laser = gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        LaserUpdate();
    }

    private void LaserUpdate()
    {
        CastRay(transform.position, transform.right);
        laserIndices.Clear();
    }

    private void CastRay(Vector3 position, Vector3 direction)
    {
        laserIndices.Add(position);
        Ray ray = new Ray(position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, maxLaserLength, 1))
        {
            CheckHit(hit, direction);
        }
        else
        {
            laserIndices.Add(ray.GetPoint(maxLaserLength));
            UpdateLaserBeam();
        }
    }

    private void CheckHit(RaycastHit hitInfo, Vector3 direction)
    {
        if (hitInfo.collider.gameObject.TryGetComponent(out LaserMirror _))
        {
            Vector3 pos = hitInfo.point;
            Vector3 dir = Vector3.Reflect(direction, hitInfo.normal);
            CastRay(pos, dir);
        }

        if (hitInfo.collider.gameObject.TryGetComponent(out LaserTarget _))
        {
            laserIndices.Add(hitInfo.point);
            UpdateLaserBeam();
        }
        else
        {
            laserIndices.Add(hitInfo.point);
            UpdateLaserBeam();
        }
    }

    private void UpdateLaserBeam()
    {
        int length = laserIndices.Count;
        laser.positionCount = length;

        for (int i = 0; i < length; i++)
            laser.SetPosition(i, laserIndices[i]);
    }
}
