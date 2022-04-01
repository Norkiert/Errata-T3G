using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanvasVertical : MonoBehaviour
{
    [SerializeField] private float slideRange = 100f;
    [SerializeField] private float clickDelay = 0.7f;
    [SerializeField] private float slideTime = 1f;
    [SerializeField] private GameObject mouseDefault;
    [SerializeField] private GameObject mouseLMB;

    private RectTransform lmbTransform;
    private Vector2 mouseStart;

    void Awake()
    {
        lmbTransform = mouseLMB.GetComponent<RectTransform>();
        mouseStart = mouseDefault.GetComponent<RectTransform>().anchoredPosition;
    }

    private void OnEnable()
    {
        mouseLMB.transform.DOKill();
        lmbTransform.anchoredPosition = mouseStart;
        mouseDefault.SetActive(true);
        mouseLMB.SetActive(false);
        mirrorToolTip = MirrorToolTip();
        StartCoroutine(mirrorToolTip);
    }
    private void OnDisable()
    {
        if (mirrorToolTip != null)
        {
            StopCoroutine(mirrorToolTip);
        }
    }

    IEnumerator mirrorToolTip;
    private IEnumerator MirrorToolTip()
    {
        yield return new WaitForSeconds(clickDelay);
        mouseDefault.SetActive(false);
        mouseLMB.SetActive(true);
        mouseLMB.transform.DOMoveY(mouseLMB.transform.position.y + slideRange, slideTime);
        yield return new WaitForSeconds(slideTime);
        mouseLMB.transform.DOMoveY(mouseLMB.transform.position.y - 2 * slideRange, 2 * slideTime);
        yield return new WaitForSeconds(2 * slideTime);
        mouseLMB.transform.DOMoveY(mouseLMB.transform.position.y + slideRange, slideTime);
        yield return new WaitForSeconds(slideTime);
        this.gameObject.SetActive(false);
    }
}
