using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private LineRenderer line;
    [SerializeField] private List<GameObject> points;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = points.Count;
    }

    void Update()
    {
        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i].transform.position);
        }
    }
}
