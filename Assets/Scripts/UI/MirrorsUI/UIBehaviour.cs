using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIBehaviour : MonoBehaviour
{
    [Header("References to Canvases")]
    [SerializeField] private CanvasVertical vert;
    [SerializeField] private CanvasHorizontal horiz;

    [Header("Script parameters")]
    [SerializeField] private float delayToShowToolTip = 3f;

    private void Start()
    {
        StopMirrorUI();
    }
    /// <summary>
    /// public void that can be called to start Couroutine with given parameters
    /// </summary>
    /// <param name="hor">bool declaring if horizontal ToolTip should be didsplayed</param>
    /// <param name="ver">bool declaring if vertical ToolTip should be didsplayed</param>
    /// 
    public void StartMirrorUI(bool hor, bool ver)
    {
        turnOnCanvas = TurnOnCanvas(hor, ver);
        StartCoroutine(turnOnCanvas);
    }
    public void StopMirrorUI()
    {
        if (turnOnCanvas != null)
            StopCoroutine(turnOnCanvas);
        if (vert.isActiveAndEnabled)
            vert.gameObject.SetActive(false);
        if (horiz.isActiveAndEnabled)
            horiz.gameObject.SetActive(false);
    }

    /// <summary>
    /// Ienumerator that wait given delay, and then showing ToolTip UI depending on given parameters
    /// </summary>
    private IEnumerator turnOnCanvas;
    private IEnumerator TurnOnCanvas(bool hor, bool ver)
    {
        yield return new WaitForSeconds(delayToShowToolTip);
        if (hor)
        {
            horiz.gameObject.SetActive(true);
        }
        if (ver)
        {
            vert.gameObject.SetActive(true);
        }
    }
    
}
