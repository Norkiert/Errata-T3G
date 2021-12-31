using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam
{
    Vector3 position, direction;

    GameObject laserObject;
    LineRenderer laser;

    List<Vector3> laserIndices = new List<Vector3>();
    [SerializeField] private Color laserColor = Color.red;
    private float maxLength = 100f;
    public LaserBeam(Vector3 position, Vector3 direction, Material material, float maxLaserLength)
    {
        laser = new LineRenderer();
        laserObject = new GameObject();
        laserObject.name = "Laser Beam";

        this.position = position;
        this.direction = direction;

        laser = laserObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        laser.startWidth = 0.1f;
        laser.endWidth = 0.1f;
        laser.material = material;
        laser.startColor = laserColor;
        laser.endColor = laserColor;
        maxLength = maxLaserLength;

        //CastRay(position, direction, laser);
    }
    public void LaserUpdate(Vector3 position, Vector3 direction)
    {
        CastRay(position, direction);
        laserIndices.Clear();
    }
    void CastRay(Vector3 position, Vector3 direction)
    {
        LineRenderer laser = laserObject.GetComponent<LineRenderer>();
        laserIndices.Add(position);
        Ray ray = new Ray(position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxLength, 1))
        {
            CheckHit(hit, direction);
        }
        else
        {
            laserIndices.Add(ray.GetPoint(30));
            UpdateLaserBeam();
        }
    }

    void CheckHit(RaycastHit hitInfo, Vector3 direction)
    {
        LineRenderer laser = laserObject.GetComponent<LineRenderer>();
        if (hitInfo.collider.gameObject.GetComponent<IsMirror>() != null)
        {
            Vector3 pos = hitInfo.point;
            Vector3 dir = Vector3.Reflect(direction, hitInfo.normal);
            CastRay(pos, dir);
        }
        if (hitInfo.collider.gameObject.GetComponent<IsTarget>() != null)
        {
            laserColor = Color.green;
            laser.startColor = laserColor;
            laser.endColor = laserColor;
            laserIndices.Add(hitInfo.point);
            UpdateLaserBeam();
        }
        else
        {
            laserIndices.Add(hitInfo.point);
            UpdateLaserBeam();
        }
    }

    void UpdateLaserBeam()
    {
        int count = 0;
        laser.positionCount = laserIndices.Count;

        foreach (Vector3 idx in laserIndices)
        {
            laser.SetPosition(count, idx);
            count++;
        }
    }
}
