using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderHandler : MonoBehaviour
{

    [Header("Thunders borders")]
    [SerializeField] private float nonThunderAreaSize = 300f;
    [SerializeField] private float sizeOnX = 1000f;
    [SerializeField] private float sizeOnZ = 1000f;

    [Header("Particle System with Thunders")]

    [SerializeField] private ParticleSystem thundersParticleSystem;

    [Header("Thunders Options")]

    [SerializeField] private float thunderLifeTime = 0.4f;
    [SerializeField] private float minimumTimeSinceLastThunder=0.9f;
    [SerializeField] private float maximumTimeSinceLastThunder = 1.4f;

    private void Start()
    {
        StartCoroutine(WaitForNextThunder(minimumTimeSinceLastThunder,maximumTimeSinceLastThunder));
    }
    private IEnumerator WaitForNextThunder(float min, float max)
    {
        while(true)
        {
            yield return new WaitForSeconds((Random.value*1000)%(max-min)+min);
            PickNextThunderPosition();
            thundersParticleSystem.Play();
            yield return new WaitForSeconds(thunderLifeTime);
            thundersParticleSystem.Stop();
        }
    }
    private void PickNextThunderPosition()
    {
        float xCoord = (10000 * Random.value) % sizeOnX - sizeOnX/2;
        float zCoord = (10000 * Random.value) % sizeOnZ - sizeOnZ/2;
        xCoord = xCoord < 0 ? xCoord - nonThunderAreaSize/2 : xCoord + nonThunderAreaSize/2;
        zCoord = zCoord < 0 ? zCoord - nonThunderAreaSize/2 : zCoord + nonThunderAreaSize/2;
        transform.position = new Vector3(xCoord,transform.position.y,zCoord);
    }
}
