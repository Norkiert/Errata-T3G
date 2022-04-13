using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CableRender : MonoBehaviour
{
    [SerializeField] bool drawLines = false;
    [SerializeField, ShowIf(nameof(drawLines))] private GameObject targetTower;

    private void Start()
    {
        if(drawLines)
        {
            GameObject[] targets = new GameObject[6];
            GameObject[] towerPoints = new GameObject[6];
            LineRenderer[] lines = new LineRenderer[6];
            for (int i = 1; i <= 6; i++)
            {
                targets[i - 1] = targetTower.transform.Find(("Target" + i).ToString()).gameObject;
                towerPoints[i - 1] = transform.Find(("Target" + i).ToString()).gameObject;
                lines[i - 1] = towerPoints[i - 1].GetComponent<LineRenderer>();

                lines[i - 1].SetPosition(0, towerPoints[i - 1].transform.position);
                lines[i - 1].SetPosition(1, targets[i - 1].transform.position);
            }
        }
    }
}
