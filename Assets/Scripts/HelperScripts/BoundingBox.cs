using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

[ExecuteInEditMode()]

[RequireComponent(typeof(Collider))]
public class BoundingBox : MonoBehaviour
{
    [SerializeField] protected bool drawBox = true;
    [SerializeField] protected bool drawOnlyOnSelection = true;
    [SerializeField] protected bool ignoreScale = true;
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

    public Vector3 Extents { get { return XpYpZp; } private set { } }

    protected void Update()
    {
        UpdatePositions();
#if UNITY_EDITOR
        if (drawBox)
            DrawBox();
#endif
        UpdateForDisplay();
    }
    protected void UpdatePositions()
    {
        Bounds bounds = GetComponent<Collider>().bounds;

        center = bounds.center;
        Vector3 extents = bounds.extents;

        XpYpZp = new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z);
        XpYpZm = new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z);
        XpYmZp = new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z);
        XpYmZm = new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z);
        XmYpZp = new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z);
        XmYpZm = new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z);
        XmYmZp = new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z);
        XmYmZm = new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z);
    }
    protected void UpdateForDisplay()
    {
        if (displayAsRelative)
        {
            XpYpZp -= center;
            XpYpZm -= center;
            XpYmZp -= center;
            XpYmZm -= center;
            XmYpZp -= center;
            XmYpZm -= center;
            XmYmZp -= center;
            XmYmZm -= center;
        }
        if (ignoreScale)
        {
            XpYpZp = new Vector3(XpYpZp.x / transform.lossyScale.x, XpYpZp.y / transform.lossyScale.y, XpYpZp.z / transform.lossyScale.z);
            XpYpZm = new Vector3(XpYpZm.x / transform.lossyScale.x, XpYpZm.y / transform.lossyScale.y, XpYpZm.z / transform.lossyScale.z);
            XpYmZp = new Vector3(XpYmZp.x / transform.lossyScale.x, XpYmZp.y / transform.lossyScale.y, XpYmZp.z / transform.lossyScale.z);
            XpYmZm = new Vector3(XpYmZm.x / transform.lossyScale.x, XpYmZm.y / transform.lossyScale.y, XpYmZm.z / transform.lossyScale.z);
            XmYpZp = new Vector3(XmYpZp.x / transform.lossyScale.x, XmYpZp.y / transform.lossyScale.y, XmYpZp.z / transform.lossyScale.z);
            XmYpZm = new Vector3(XmYpZm.x / transform.lossyScale.x, XmYpZm.y / transform.lossyScale.y, XmYpZm.z / transform.lossyScale.z);
            XmYmZp = new Vector3(XmYmZp.x / transform.lossyScale.x, XmYmZp.y / transform.lossyScale.y, XmYmZp.z / transform.lossyScale.z);
            XmYmZm = new Vector3(XmYmZm.x / transform.lossyScale.x, XmYmZm.y / transform.lossyScale.y, XmYmZm.z / transform.lossyScale.z);

        }
    }
#if UNITY_EDITOR
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
#endif
}