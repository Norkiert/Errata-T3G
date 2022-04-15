using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuideCube;
using Pathfinding;
using NaughtyAttributes;

public class HubGCubeBehaviours : MonoBehaviour
{
    [SerializeField, Required] private Point cubeDockingPoint;

    [Header("Dialogue")]
    [SerializeField] private bool startFirstDialogue = false;
    [SerializeField] private float firstDialoguedealy = 1f;
    [SerializeField, Required] private Point cubeDialoguePoint;
    [SerializeField, Required] private TextAsset dialogueText;


    private GCubeController cubeController;


    public void SetStartDialuge(bool value) => startFirstDialogue = value;


    private void Awake()
    {
        cubeController = FindObjectOfType<GCubeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (startFirstDialogue)
                Invoke(nameof(StartFirstDialogue), firstDialoguedealy);
            else
                cubeController.SetState(new GCSGoTo(cubeController, cubeDockingPoint.Position), new GCSIdle(cubeController));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            cubeController.SetState(new GCSFollowPlayer(cubeController));
        }
    }

    private void StartFirstDialogue()
    {
        startFirstDialogue = false;
        cubeController.SetState(new GCSGoTo(cubeController, cubeDialoguePoint.Position),
            new List<GCubeState> {
            new GCSDialogue(cubeController, dialogueText, 50f),
            new GCSFollowPlayer(cubeController)
            });
    }
}
