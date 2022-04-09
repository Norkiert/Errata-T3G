using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuideCube;
using Pathfinding;
using NaughtyAttributes;

public class HubGCubeBehaviours : MonoBehaviour
{
    [SerializeField, Required] private Point cubeDockingPoint;
    [SerializeField, Required] private Point cubeDialoguePoint;

    private GCubeController cubeController;
    private bool startFirstDialogue = true;

    private void Awake()
    {
        cubeController = FindObjectOfType<GCubeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (startFirstDialogue)
                StartFirstDialogue();
            else
            {
                cubeController.SetState(new GCSGoTo(cubeController, cubeDockingPoint.Position), new List<GCubeState>
                {
                    new GCSIdle(cubeController)
                });
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            cubeController.SetFollowPlayer();
        }
    }

    private void StartFirstDialogue()
    {
        startFirstDialogue = false;
        cubeController.SetState(new GCSGoTo(cubeController, cubeDialoguePoint.Position));
    }
}
