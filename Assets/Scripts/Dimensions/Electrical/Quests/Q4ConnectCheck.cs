using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class Q4ConnectCheck : MonoBehaviour
{
    [ReadOnly ]public bool q4Done;

    [SerializeField] private List<Connector> connectorsToCheck;
    [SerializeField] private float delay = 1f;
    void Start()
    {
        if (checkDone != null)
            StopCoroutine(checkDone);
        checkDone = CheckDone();
        StartCoroutine(checkDone);
    }

    private IEnumerator checkDone;

    private IEnumerator CheckDone()
    {
        while(!ConnectorsState())
        {
            yield return new WaitForSeconds(delay);
        }
        q4Done = true;
        if (checkUnDone != null)
            StopCoroutine(checkUnDone);
        checkUnDone = CheckUnDone();
        StartCoroutine(checkUnDone);
    }
    private IEnumerator checkUnDone;

    private IEnumerator CheckUnDone()
    {
        while (ConnectorsState())
        {
            yield return new WaitForSeconds(delay*2);
        }
        q4Done = true;
        if (checkDone != null)
            StopCoroutine(checkDone);
        checkDone = CheckDone();
        StartCoroutine(checkDone);
    }
    private bool ConnectorsState()
    {
        bool done = true;
        foreach(Connector con in connectorsToCheck)
        {
            if (!con.IsConnectedRight)
                done = false;
        }
        return done;
    }
}
