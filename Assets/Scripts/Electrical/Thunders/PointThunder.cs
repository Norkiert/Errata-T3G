using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Audio;

public class PointThunder : MonoBehaviour
{
    [Header("Target Particle Systems")]
    [SerializeField, ReadOnly] private ParticleSystem thunder;
    [SerializeField, ReadOnly] private ParticleSystem thunderBeam;
    [SerializeField, ReadOnly] private ParticleSystem groundBeam;

    [Header("Thunder options")]
    [SerializeField] private float beamDelay = 0.2f;
    [SerializeField] private float particlesLifetime = 0.3f;
    [SerializeField] private float minRandomThunderSpawnTime = 5f;
    [SerializeField] private float maxRandomThunderSpawnTime = 10f;
    [SerializeField] private List<GameObject> targetsToHit;

    [Header("Sounds")]
    [SerializeField] List<AudioClipSO> clips = new List<AudioClipSO>();

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
        transform.position = new Vector3(target.x,target.y*2,target.z);
        thunder.Play();
        yield return new WaitForSeconds(beamDelay);
        thunderBeam.Play();
        groundBeam.Play();
        yield return new WaitForSeconds(particlesLifetime);
        thunder.Stop();
        thunderBeam.Stop();
        groundBeam.Stop();
        PlayStormSound();
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
    
    private void PlayStormSound()
    {
        int numb = Mathf.CeilToInt((Random.value * 1000) % (clips.Count - 1));
        AudioManager.PlaySFX(clips[numb]);
    }

    #region -public functions-
    public void SpawnThunder(Vector3 targetPosition)
    {
        StopCoroutine(TimerToNextRandomThunder(0));
        StartCoroutine(ThunderStart(new Vector3(targetPosition.x,targetPosition.y+30f,targetPosition.z)));
    }
    #endregion
}
