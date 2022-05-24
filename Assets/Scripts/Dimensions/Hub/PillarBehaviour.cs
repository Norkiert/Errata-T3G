using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class PillarBehaviour : MonoBehaviour
{
    [SerializeField, ReadOnly] private Connector conToCheck;
    [SerializeField] private string obToFind;
    [SerializeField] private ParticleSystem partSys;
    [SerializeField] private ParticleSystem partSysRed;
    [SerializeField] private Dimension dimensionToCheck;
    [SerializeField] private Renderer pillar;

    [SerializeField] Material matConnected;
    [SerializeField] Material matDisconnected;

    [SerializeField] private float checkStep = 1f;
    void Start()
    {
        if (getConnectors != null)
            StopCoroutine(getConnectors);
        getConnectors = GetConnectors();
        StartCoroutine(getConnectors);
        partSys.Play();
    }

    private IEnumerator checkConnect;

    private IEnumerator CheckConnect()
    {
        int counter = 0;
        int counter2 = 0;
        while (!conToCheck.IsConnectedRight)
        {
            yield return new WaitForSeconds(checkStep);
            if (!SaveManager.IsLevelFinished(dimensionToCheck)&&counter2>4)
            {
                if(partSys.isPlaying&&counter>1)
                {
                    partSys.Stop();
                    partSysRed.Play();
                    counter = 0;
                }else
                {
                    partSysRed.Stop();
                    partSys.Play();
                    counter++ ;
                }
                counter2 = 0;
            }
            counter2++;
        }
        partSys.Stop();
        partSysRed.Stop();
        pillar.material = matDisconnected;
        if (checkDisconnect != null)
            StopCoroutine(checkDisconnect);
        checkDisconnect = CheckDisconnect();
        StartCoroutine(checkDisconnect);
    }
    private IEnumerator checkDisconnect;

    private IEnumerator CheckDisconnect()
    {
        while (conToCheck.IsConnectedRight)
            yield return new WaitForSeconds(checkStep);
        partSys.Play();
        pillar.material = matConnected;
        if (checkConnect != null)
            StopCoroutine(checkConnect);
        checkConnect = CheckConnect();
        StartCoroutine(checkConnect);
    }


    private IEnumerator getConnectors;
    private IEnumerator GetConnectors()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject ob = GameObject.Find(obToFind);
        if(ob.TryGetComponent<Connector>(out _)&& conToCheck==null)
            conToCheck = ob.GetComponent<Connector>();
        if (checkConnect != null)
            StopCoroutine(checkConnect);
        checkConnect = CheckConnect();
        StartCoroutine(checkConnect);
    }
}
