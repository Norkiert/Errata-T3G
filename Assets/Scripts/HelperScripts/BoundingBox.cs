using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

[ExecuteInEditMode()]

[RequireComponent(typeof(Collider))]
public class BoundingBox : MonoBehaviour
{
    [SerializeField] protected bool drawBox = true;
    [SerializeField] protected bool drawOnlyOnSelection = true;
    [SerializeField] protected Color color = Color.green;

    [SerializeField] [ReadOnly] protected Vector3 center;
    [SerializeField] protected bool displayAsRelative = true;
    // p - plus
    // m - minus
    [SerializeField] [ReadOnly] protected Vector3 XpYpZp;
    [SerializeField] [ReadOnly] protected Vector3 XpYpZm;
    [SerializeField] [ReadOnly] protected Vector3 XpYmZp;
    [SerializeField] [ReadOnly] protected Vector3 XpYmZm;
    [SerializeField] [ReadOnly] protected Vector3 XmYpZp;
    [SerializeField] [ReadOnly] protected Vector3 XmYpZm;
    [SerializeField] [ReadOnly] protected Vector3 XmYmZp;
    [SerializeField] [ReadOnly] protected Vector3 XmYmZm;

    protected void Update()
    {
        UpdatePositions();
        if (drawBox)
            DrawBox();
    }
    protected void UpdatePositions()
    {
        Bounds bounds = GetComponent<Collider>().bounds;

        center = bounds.center;
        Vector3 extents = bounds.extents;

        if (displayAsRelative)
        {
            XpYpZp = new Vector3(+extents.x, +extents.y, +extents.z);
            XpYpZm = new Vector3(+extents.x, +extents.y, -extents.z);
            XpYmZp = new Vector3(+extents.x, -extents.y, +extents.z);
            XpYmZm = new Vector3(+extents.x, -extents.y, -extents.z);
            XmYpZp = new Vector3(-extents.x, +extents.y, +extents.z);
            XmYpZm = new Vector3(-extents.x, +extents.y, -extents.z);
            XmYmZp = new Vector3(-extents.x, -extents.y, +extents.z);
            XmYmZm = new Vector3(-extents.x, -extents.y, -extents.z);
        }
        else
        {
            XpYpZp = new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z);
            XpYpZm = new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z);
            XpYmZp = new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z);
            XpYmZm = new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z);
            XmYpZp = new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z);
            XmYpZm = new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z);
            XmYmZp = new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z);
            XmYmZm = new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z);
        }
    }
    protected void DrawBox()
    {
        if (drawOnlyOnSelection)
        {
            var array = Selection.GetFiltered<BoundingBox>(SelectionMode.Assets);
            foreach (var selection in array)
            {
                if (selection == this)
                {
                    goto doDrawing;
                }
            }
            return;
        }
    doDrawing:
        if (displayAsRelative)
        {
            // X lines
            Debug.DrawLine(XpYpZp + center, XmYpZp + center, color);
            Debug.DrawLine(XpYpZm + center, XmYpZm + center, color);
            Debug.DrawLine(XpYmZp + center, XmYmZp + center, color);
            Debug.DrawLine(XpYmZm + center, XmYmZm + center, color);
            // Y lines
            Debug.DrawLine(XpYpZp + center, XpYmZp + center, color);
            Debug.DrawLine(XpYpZm + center, XpYmZm + center, color);
            Debug.DrawLine(XmYpZp + center, XmYmZp + center, color);
            Debug.DrawLine(XmYpZm + center, XmYmZm + center, color);
            // Z lines
            Debug.DrawLine(XpYpZp + center, XpYpZm + center, color);
            Debug.DrawLine(XpYmZp + center, XpYmZm + center, color);
            Debug.DrawLine(XmYpZp + center, XmYpZm + center, color);
            Debug.DrawLine(XmYmZp + center, XmYmZm + center, color);
        }
        else
        {
            // X lines
            Debug.DrawLine(XpYpZp, XmYpZp, color);
            Debug.DrawLine(XpYpZm, XmYpZm, color);
            Debug.DrawLine(XpYmZp, XmYmZp, color);
            Debug.DrawLine(XpYmZm, XmYmZm, color);
            // Y lines
            Debug.DrawLine(XpYpZp, XpYmZp, color);
            Debug.DrawLine(XpYpZm, XpYmZm, color);
            Debug.DrawLine(XmYpZp, XmYmZp, color);
            Debug.DrawLine(XmYpZm, XmYmZm, color);
            // Z lines
            Debug.DrawLine(XpYpZp, XpYpZm, color);
            Debug.DrawLine(XpYmZp, XpYmZm, color);
            Debug.DrawLine(XmYpZp, XmYpZm, color);
            Debug.DrawLine(XmYmZp, XmYmZm, color);
        }
    }
}