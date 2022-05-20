using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;


public class FromSteampunk : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private List<Transform> targets;
    [SerializeField] private float flySpeed = 10f;
    [SerializeField] private GameObject laserFinal;

    [Header("Timer settings")]
    [SerializeField] private float minTimeToSpawn = 3f;
    [SerializeField] private float maxTimeToSpawn = 10f;

    void Start()
    {
        ball.SetActive(false);
        if(!SaveManager.isLevelFinished(Dimension.Steampunk))
        {
            if (timerToSpawn != null)
                StopCoroutine(timerToSpawn);
            timerToSpawn = TimerToSpawn();
            StartCoroutine(timerToSpawn);
        }
        StartCoroutine(IsFinished());
    }
       
    private Vector3 PickTarget()
    {
        int bul = (int)((Random.value * 10000000)% targets.Count);
        return (targets[bul].position + new Vector3(0, 100, 0));
    }

    private IEnumerator timerToSpawn;

    private IEnumerator TimerToSpawn()
    {
        float time = ((Random.value * 100000) % (maxTimeToSpawn - minTimeToSpawn)) + minTimeToSpawn;
        yield return new WaitForSeconds(time);
        ball.transform.position = PickTarget();
        if (flyingBall != null)
            StopCoroutine(flyingBall);
        flyingBall = FlyingBall();
        StartCoroutine(flyingBall);
    }

    private IEnumerator flyingBall;

    private IEnumerator FlyingBall()
    {
        ball.SetActive(true);
        ball.transform.DOScale(new Vector3(4, 4, 4), 0.1f);
        while(ball.transform.position.y>-15)
        {
            ball.transform.position -= new Vector3(0, flySpeed * Time.deltaTime, 0);
            yield return null;
        }
        ball.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 2f);
        yield return new WaitForSeconds(2f);
        ball.SetActive(false);
        if (timerToSpawn != null)
            StopCoroutine(timerToSpawn);
        timerToSpawn = TimerToSpawn();
        StartCoroutine(timerToSpawn);
    }

    private void OnDisable()
    {
        StopAllCoroutines();        
    }

    private void stopEverything()
    {
        StopAllCoroutines();
        ball.transform.DOScale(new Vector3(4, 4, 4), 0.1f);
        ball.SetActive(false);
    }
    private IEnumerator IsFinished()
    {
        while (!laserFinal.activeSelf)
            yield return new WaitForSeconds(2f);
        stopEverything();
    }
}
