using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ElectricalQuestCompleted : MonoBehaviour
{
    [ReadOnly] public bool Q1State;
    [ReadOnly] public bool Q2State;
    [ReadOnly] public bool Q3State;
    [ReadOnly] public bool Q4State;

    [Header("Q1 Objects")]
    [SerializeField] private Collider sillQ1Collider;

    [Header("Q2 Objects")]
    [SerializeField] private Connector conQ2_1;
    [SerializeField] private Connector conQ2_2;

    [Header("Q3 Objects")]
    [SerializeField] ParticleSystem particleQ3;

    [Header("Q4 Objects")]
    [SerializeField] private Q4ConnectCheck q4Con;


    [Header("Panel objects and materials")]
    [SerializeField] private Material unDoneMaterial;
    [SerializeField] private Material doneMaterial;
    [SerializeField] private Renderer q1Lamp;
    [SerializeField] private Renderer q2Lamp;
    [SerializeField] private Renderer q3Lamp;
    [SerializeField] private Renderer q4Lamp;

    [SerializeField] private float checkDelay = 2f;
    void Start()
    {
        Q1State = false;
        Q2State = false;
        Q3State = false;
        Q4State = false;

        q1Lamp.material = unDoneMaterial;
        q2Lamp.material = unDoneMaterial;
        q3Lamp.material = unDoneMaterial;
        q4Lamp.material = unDoneMaterial;

        q4Con = FindObjectOfType<Q4ConnectCheck>();
        CheckQuests();
        if (counterToCheck != null)
            StopCoroutine(counterToCheck);
        counterToCheck = CounterToCheck();
        StartCoroutine(counterToCheck);
    }


    private IEnumerator counterToCheck;

    private IEnumerator CounterToCheck()
    {
        yield return new WaitForSeconds(checkDelay);
        CheckQuests();
    }
    

    private void CheckQuests()
    {
        //Check Q1
        if (!sillQ1Collider.enabled)
            Q1State = true;
        else
            Q1State = false;
        //Check Q2
        if (conQ2_1.IsConnectedRight && conQ2_2.IsConnectedRight)
            Q2State = true;
        else
            Q2State = false;
        //CheckQ3
        if (particleQ3.isPlaying)
            Q3State = true;
        else
            Q3State = false;
        //CheckQ4
        if (q4Con.q4Done)
            Q4State = true;
        else
            Q4State = false;

        ChangeLampsMaterials(Q1State, Q2State, Q3State, Q4State);
        if (counterToCheck != null)
            StopCoroutine(counterToCheck);
        counterToCheck = CounterToCheck();
        StartCoroutine(counterToCheck);
    }

    /// <summary>
    /// Change material for declared objects to given material depending on quest completition
    /// </summary>
    /// <param name="q1">bool defining q1 state</param>
    /// <param name="q2">bool defining q2 state</param>
    /// <param name="q3">bool defining q3 state</param>
    /// <param name="q4">bool defining q4 state</param>
    private void ChangeLampsMaterials(bool q1,bool q2, bool q3, bool q4)
    {
        if (q1)
            q1Lamp.material = doneMaterial;
        else
            q1Lamp.material = unDoneMaterial;
        if (q2)
            q2Lamp.material = doneMaterial;
        else
            q2Lamp.material = unDoneMaterial;
        if (q3)
            q3Lamp.material = doneMaterial;
        else
            q3Lamp.material = unDoneMaterial;
        if (q4)
            q4Lamp.material = doneMaterial;
        else
            q4Lamp.material = unDoneMaterial;
    }



}
