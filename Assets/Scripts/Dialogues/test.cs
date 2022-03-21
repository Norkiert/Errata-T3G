using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : Clickable
{
    [SerializeField] private TextAsset inkJSON;

    void Start()
    {
        OnClick += Clicked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked()
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }
}
