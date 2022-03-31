using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private CanvasVertical vert;
    [SerializeField] private CanvasHorizontal horiz;

    private RectTransform lmbTransform;
    private Vector2 mouseStart;

    private void Start()
    {
        StopMirrorUI();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="side"> 1 for vertical 2 for horizontal</param>
    public void StartMirrorUI(bool hor, bool ver)
    {
        if (hor)
        {
            horiz.gameObject.SetActive(true);
        }
        if (ver)
        {
            vert.gameObject.SetActive(true);
        }
    }
    public void StopMirrorUI()
    {
        if(vert.isActiveAndEnabled)
            vert.gameObject.SetActive(false);
        if (horiz.isActiveAndEnabled)
            horiz.gameObject.SetActive(false);
    }
}
