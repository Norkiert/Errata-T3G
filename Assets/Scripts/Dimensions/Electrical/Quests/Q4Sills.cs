using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q4Sills : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> partSys;
    [SerializeField] private List<Collider> coliders;
    [SerializeField] private Connector con1;
    [SerializeField] private Connector con2;
    void Start()
    {
        if (q4SillsTurnOff != null)
            StopCoroutine(q4SillsTurnOff);
        q4SillsTurnOff = Q4SillsTurnOff();
        StartCoroutine(q4SillsTurnOff);
    }

    private IEnumerator q4SillsTurnOff;

    private IEnumerator Q4SillsTurnOff()
    {
        while(!(con1.IsConnectedRight&&con2.IsConnectedRight))
        {
            yield return new WaitForSeconds(2f);
        }
        if(coliders.Count>0&&partSys.Count>0)
        {
            foreach (Collider col in coliders)
                col.enabled = false;
            foreach (ParticleSystem part in partSys)
                part.Stop();
        }
    }
}
