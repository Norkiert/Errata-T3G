using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using GuideCube;

namespace Dialogues
{
    public class DialogueZone : MonoBehaviour
    {
        [SerializeField, Required] private TextAsset dialogueText;
        [SerializeField] private bool playOnEnter;
        [SerializeField] private bool idleInPoint;
        [SerializeField, ShowIf(nameof(idleInPoint)), Required] private Point idlePoint;

        private readonly List<GCubeState> states = new List<GCubeState>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController _))
            {
                DialogueInkKeeper.AddNewText(dialogueText);

                states.Clear();

                if (idleInPoint)
                {
                    if (idlePoint == null)
                        Debug.LogWarning($"{name}: Idle point is not set!");
                    else
                        states.Add(new GCSGoTo(GCubeController.Instance, idlePoint.Position));
                }

                if (playOnEnter)
                    states.Add(new GCSDialogue(GCubeController.Instance, dialogueText, 100));

                if (states.Count > 0)
                    GCubeController.Instance.SetState(states);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController _))
            {
                DialogueInkKeeper.RemoveText(dialogueText);

                if (playOnEnter)
                {
                    if (states.Contains(GCubeController.Instance.CurrentState))
                        GCubeController.Instance.SetState(new GCSFollowPlayer(GCubeController.Instance));
                }

                states.Clear();
            }
        }
    }
}