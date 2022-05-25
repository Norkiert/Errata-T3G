using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserRotator : MonoBehaviour
{
    [SerializeField] private float delayToMove = 3f;
    [SerializeField] private float moveTime = 4f;

    void Start()
    {
        if (rotator != null)
            StopCoroutine(rotator);
        rotator = Rotator();
        StartCoroutine(rotator);
    }

    private IEnumerator rotator;

    private IEnumerator Rotator()
    {
        while(this.enabled)
        {
            yield return new WaitForSeconds(delayToMove+moveTime);
            transform.DOLocalRotate(new Vector3(0, 60, -15), moveTime);
            yield return new WaitForSeconds(delayToMove+moveTime);
            transform.DOLocalRotate(new Vector3(0, 93, -15), moveTime);

        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        transform.DOKill();
    }

}
