using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CableRender : MonoBehaviour
{
    [SerializeField] private GameObject targetTower;

    [SerializeField, Min(0)] private int additionalPoints = 4;
    [SerializeField, Min(0)] private float dangle = 0.4f;
    [SerializeField, Range(0,1)] private float dangleScale = 6f;
    [SerializeField] private Vector3 dangleDirection = Vector3.down;

    [Button("Set lines")]
    private void SetLines()
    {
        for (int i = 0; i < 6; i++)
        {
            string name = $"Target{i + 1}";
            Transform thisPoint = transform.Find(name);
            Transform targetPoint = targetTower.transform.Find(name);

            if (thisPoint == null || targetPoint == null)
            {
                Debug.LogWarning($"Dont found \"{name}\" point");
                continue;
            }

            LineRenderer line = thisPoint.GetComponent<LineRenderer>();

            Vector3 startPos = thisPoint.position;
            Vector3 endPos = targetPoint.position;

            Vector3[] points = new Vector3[additionalPoints + 2];
            points[0] = startPos;
            points[additionalPoints + 1] = endPos;

            Vector3 tPos = startPos;
            Vector3 xOffset = (endPos - startPos) / (float)(additionalPoints + 1);
            for (int ip = 1; ip <= additionalPoints; ip++)
            {
                tPos += xOffset;
                points[ip] = tPos;
            }

            float tDangle = 0;
            for (int ip = 1; ip <= additionalPoints / 2; ip++)
            {
                tDangle += (2 * dangle - tDangle) * dangleScale * dangle / (float)additionalPoints;
                points[ip] += dangleDirection * tDangle;
                points[additionalPoints - ip + 1] += dangleDirection * tDangle;
            }
            if (additionalPoints % 2 == 1)
                points[additionalPoints / 2 + 1] += dangleDirection * tDangle;

            line.positionCount = points.Length;
            line.SetPositions(points);
        }
    }

    private void Start()
    {
        SetLines();
    }
}
