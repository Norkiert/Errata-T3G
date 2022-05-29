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

        [Header("Play on enter")]
        [SerializeField] private bool playOnEnter;
        [SerializeField, ShowIf(nameof(playOnEnter)), Required] private TextAsset dialogueOnEnterText;
        [SerializeField, Min(0), ShowIf(nameof(playOnEnter))] private float dialogueCloseDistOnEnter = 100;
        [SerializeField, ShowIf(nameof(playOnEnter)),] private bool playOnlyOneTimeOnEnter = false;
        [SerializeField, ShowIf(EConditionOperator.And, nameof(playOnEnter), nameof(playOnlyOneTimeOnEnter))] private string uniqueSaveKeyOnEnter = "undefined";


        [Header("Idel pointr")]
        [SerializeField] private bool idleInPoint;
        [SerializeField, ShowIf(nameof(idleInPoint)), Required] private Point idlePoint;


        private readonly List<GCubeState> states = new List<GCubeState>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController _))
                EnterDialogueZone();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController _))
                ExitDialogueZone();
        }

        private void OnDisable()
        {
            ExitDialogueZone();
        }

        private void EnterDialogueZone()
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

            if (playOnEnter && (!playOnlyOneTimeOnEnter || !HroberPrefs.ReadBool(uniqueSaveKeyOnEnter, false)))
            {
                if (playOnlyOneTimeOnEnter)
                    StartCoroutine(nameof(WaitForDialgue));

                states.Add(new GCSDialogue(GCubeController.Instance, dialogueOnEnterText, dialogueCloseDistOnEnter));
            }

            if (states.Count > 0)
                states.Add(new GCSIdle(GCubeController.Instance));

            if (states.Count > 0)
                GCubeController.Instance.SetState(states);
        }

        private void ExitDialogueZone()
        {
            DialogueInkKeeper.RemoveText(dialogueText);

            if (GCubeController.Instance != null && states.Contains(GCubeController.Instance.CurrentState) && GCubeController.Instance.CurrentState is GCSDialogue == false)
            {
                GCubeController.Instance.SetState(new GCSFollowPlayer(GCubeController.Instance));
            }

            states.Clear();

            StopCoroutine(nameof(WaitForDialgue));
        }

        private IEnumerator WaitForDialgue()
        {
            while (true)
            {
                if (DialogueManager.instance.IsDialoguePlaying)
                {
                    HroberPrefs.SaveBool(uniqueSaveKeyOnEnter, true);
                    yield break;
                }

                yield return null;
            }
        }
    }
}