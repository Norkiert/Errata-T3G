using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableLamps : MonoBehaviour
{
    [SerializeField] private List<Renderer> Q1Lamps;
    [SerializeField] private List<Renderer> Q2Lamps;
    [SerializeField] private List<Renderer> Q3Lamps;
    [SerializeField] private List<Renderer> Q4Lamps;
    [SerializeField] private ElectricalQuestCompleted quest;
    [Header("Materials")]
    [SerializeField] private Material unDoneMaterial;
    [SerializeField] private Material doneMaterial;
    void Start()
    {
        if (counterToCheck != null)
            StopCoroutine(counterToCheck);
        counterToCheck = CounterToCheck();
        StartCoroutine(counterToCheck);
    }


    private IEnumerator counterToCheck;

    private IEnumerator CounterToCheck()
    {
        while(true)
        {
            yield return new WaitForSeconds(2f);
            if (quest.Q1State)
                SetMaterialOnLamps(Q1Lamps, doneMaterial);
            else
                SetMaterialOnLamps(Q1Lamps, unDoneMaterial);

            if (quest.Q2State)
                SetMaterialOnLamps(Q2Lamps, doneMaterial);
            else
                SetMaterialOnLamps(Q2Lamps, unDoneMaterial);

            if (quest.Q3State)
                SetMaterialOnLamps(Q3Lamps, doneMaterial);
            else
                SetMaterialOnLamps(Q3Lamps, unDoneMaterial);

            if (quest.Q4State)
                SetMaterialOnLamps(Q4Lamps, doneMaterial);
            else
                SetMaterialOnLamps(Q4Lamps, unDoneMaterial);
        }    
    }
    private void SetMaterialOnLamps(List<Renderer> lamps, Material mat)
    {
        if (lamps.Count < 1)
            return;

        foreach (Renderer ren in lamps)
            ren.material = mat;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
