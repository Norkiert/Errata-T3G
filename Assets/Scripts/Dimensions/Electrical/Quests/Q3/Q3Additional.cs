using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q3Additional : MonoBehaviour
{
    [SerializeField] private ParticleSystem oldPart1;
    [SerializeField] private ParticleSystem oldPart2;
    [SerializeField] private ParticleSystem newPart1;
    [SerializeField] private ParticleSystem newPart2;

    [SerializeField] private Connector con1;
    [SerializeField] private Connector con2;
    [SerializeField] private Connector con3;
    [SerializeField] private Connector con4;
    void Start()
    {
        oldPart1 = GameObject.Find("Q3Smoke").GetComponent<ParticleSystem>();
        oldPart2 = GameObject.Find("Q3SmokeTop").GetComponent<ParticleSystem>();
        newPart1 = GameObject.Find("Q3SmokeFinal").GetComponent<ParticleSystem>();
        newPart2 = GameObject.Find("Q3SmokeTopFinal").GetComponent<ParticleSystem>();
        checkConnect = CheckConnect();
        StartCoroutine(checkConnect);
        if(!(con1.IsConnectedRight && con2.IsConnectedRight && con3.IsConnectedRight && con4.IsConnectedRight))
        {
            newPart1.Stop();
            newPart2.Stop();
            oldPart1.Play();
            oldPart2.Play();
        }else
        {
            newPart1.Play();
            newPart2.Play();
            oldPart1.Stop();
            oldPart2.Stop();
        }
    }

    private IEnumerator checkConnect;
    private IEnumerator CheckConnect()
    {
        while(!(con1.IsConnectedRight&&con2.IsConnectedRight&&con3.IsConnectedRight && con4.IsConnectedRight))
        {
            yield return new WaitForSeconds(2f);
        }
        Complete();
    }

    private IEnumerator checkDisconnect;
    private IEnumerator CheckDisconnect()
    {
        while (con1.IsConnectedRight && con2.IsConnectedRight && con3.IsConnectedRight && con4.IsConnectedRight)
        {
            yield return new WaitForSeconds(2f);
        }
        Uncomplete();
    }


    private void Complete()
    {
        if (checkConnect != null)
            StopCoroutine(checkConnect);
        oldPart1.Stop();
        oldPart2.Stop();
        newPart1.Play();
        newPart2.Play();
        checkDisconnect = CheckDisconnect();
        StartCoroutine(checkDisconnect);
    }
    private void Uncomplete()
    {
        if (checkDisconnect != null)
            StopCoroutine(checkDisconnect);
        oldPart1.Play();
        oldPart2.Play();
        newPart1.Stop();
        newPart2.Stop();
        checkConnect = CheckConnect();
        StartCoroutine(checkConnect);
    }
}
