using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PointThunder : MonoBehaviour
{
    [Header("Target Particle Systems")]
    [SerializeField, ReadOnly] private ParticleSystem thunder;
    [SerializeField, ReadOnly] private ParticleSystem thunderBeam;
    [SerializeField, ReadOnly] private ParticleSystem groundBeam;

    [Header("Thunder options")]
    [SerializeField] private float particlesLifetime = 0.3f;
    [SerializeField] private float minRandomThunderSpawnTime = 5f;
    [SerializeField] private float maxRandomThunderSpawnTime = 10f;
    [SerializeField] private List<GameObject> targetsToHit;

    private int targetsCount;
    void Start()
    {
        thunder = GameObject.Find("ThunderTrail").GetComponent <ParticleSystem>();
        thunderBeam = GameObject.Find("ThunderBeam").GetComponent<ParticleSystem>();
        groundBeam = GameObject.Find("GroundBeam").GetComponent<ParticleSystem>();
        targetsCount = targetsToHit.Count;

        StartCoroutine(TimerToNextRandomThunder(1f));
    }
    IEnumerator ThunderStart(Vector3 target)
    {
        transform.position = target;
        thunder.Play();
        yield return new WaitForSeconds(0.1f);
        thunderBeam.Play();
        groundBeam.Play();
        yield return new WaitForSeconds(particlesLifetime);
        thunder.Stop();
        thunderBeam.Stop();
        groundBeam.Stop();
        float time = (Random.value * 100000) % (maxRandomThunderSpawnTime - minRandomThunderSpawnTime) + minRandomThunderSpawnTime;
        StartCoroutine(TimerToNextRandomThunder(time));
    }

    IEnumerator TimerToNextRandomThunder(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(ThunderStart(PickObcjectToHit()));
    }

    private Vector3 PickObcjectToHit()
    {
        int num = (int)(Mathf.Ceil((Random.value*100000)%targetsCount))-1;
        return targetsToHit[num].transform.position;
    }

    #region -public functions-
    public void SpawnThunder(Vector3 targetPosition)
    {
        StopCoroutine(TimerToNextRandomThunder(0));
        StartCoroutine(ThunderStart(new Vector3(targetPosition.x,targetPosition.y+30f,targetPosition.z)));
    }
    #endregion
}
