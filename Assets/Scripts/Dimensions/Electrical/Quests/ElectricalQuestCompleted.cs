using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Logic;

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

    [Header("Logic object")]
    [SerializeField] private LogicBoolOutputValue q1Value;
    [SerializeField] private LogicBoolOutputValue q2Value;
    [SerializeField] private LogicBoolOutputValue q3Value;
    [SerializeField] private LogicBoolOutputValue q4Value;

    [Header("Settings")]
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
        Q1State = !sillQ1Collider.enabled;
        q1Value.SetValue(Q1State);

        //Check Q2
        Q2State = conQ2_1.IsConnectedRight && conQ2_2.IsConnectedRight;
        q2Value.SetValue(Q2State);

        //CheckQ3
        Q3State = particleQ3.isPlaying;
        q3Value.SetValue(Q3State);

        //CheckQ4
        if(q4Con!=null)
            Q4State = q4Con.q4Done;
        else
        {
            q4Con = FindObjectOfType<Q4ConnectCheck>();
            Q4State = q4Con.q4Done;
        }
        q4Value.SetValue(Q4State);

        ChangeLampsMaterials(q1Lamp, Q1State);
        ChangeLampsMaterials(q2Lamp, Q2State);
        ChangeLampsMaterials(q3Lamp, Q3State);
        ChangeLampsMaterials(q4Lamp, Q4State);

        if (counterToCheck != null)
            StopCoroutine(counterToCheck);
        counterToCheck = CounterToCheck();
        StartCoroutine(counterToCheck);
    }

    private void ChangeLampsMaterials(Renderer lamp, bool state)
    {
        Material mat = state ? doneMaterial : unDoneMaterial;
        if (mat != lamp.material)
            lamp.material = mat;
    }
}
