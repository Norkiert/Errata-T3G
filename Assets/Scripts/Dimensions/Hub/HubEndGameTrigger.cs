using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagment;
using System;
using UnityEngine.UI;
using Dialogues;
using GuideCube;

public class HubEndGameTrigger : MonoBehaviour
{
    [SerializeField] private HubPlayerHandler activator;
    [SerializeField] private DimensionSO mainDimension;
    [SerializeField] private Connector cableToDisconnect;
    [SerializeField] private Transform endPlayerPOsition;

    [Header("End one")]
    [SerializeField] private TextAsset dialogueText;
    [SerializeField] private Pathfinding.Point dialoguePoint;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Image backround;
    [SerializeField] private float timeForLastDialogue = 7;
    [SerializeField] private float whiteBackroundDelay = 2;
    [SerializeField] private float whiteBackroundTintInTime = 3;
    [SerializeField] private List<Light> lights = new List<Light>();
    [SerializeField] private float lightsIntensity = 300;
    [SerializeField] private float lightTintTime = 3;

    [Header("Back")]
    [SerializeField] private float whiteScreanDuration = 3f;
    [SerializeField] private Pathfinding.Point dockingPoint;
    [SerializeField] private float whiteBackroundTintOutTime = 3;

    [Header("player reaction")]
    [SerializeField] private float playerReactionDelay = 3;
    [SerializeField] private float blackBackroundTintTime = 3;

    [Header("EndScreen")]
    [SerializeField] private float endPanelDealy = 4;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private float blackButonShowDleay = 3;
    [SerializeField] private Button backToMenuButton;


    private const string saveName = "EndGameTrigger";

    private void OnEnable() => activator.OnChange += OnPlayerEnter;
    private void OnDisable() => activator.OnChange -= OnPlayerEnter;

    private void OnPlayerEnter()
    {
        if (activator.IsPlayerInHub && HroberPrefs.ReadBool(saveName, false) == false)
            StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        // wiat for save
        yield return new WaitForSeconds(0.5f);

        // check save
        if (SaveManager.AreAllLevelFinished() == false)
            yield break;

        // set bool
        HroberPrefs.SaveBool(saveName, true);

        PlayerController playerController = FindObjectOfType<PlayerController>();
        PlayerInteractions playerInteractions = playerController.GetComponent<PlayerInteractions>();
        GCubeController cubeController = FindObjectOfType<GCubeController>();

        // prepear
        {
            playerController.FreezMovement = true;
            playerInteractions.enabled = false;

            playerController.SetPosition(endPlayerPOsition.position);

            cableToDisconnect.Disconnect(false);
            DialogueManager.instance.CloseDialoguePanel();

            // load dimesnuin
            if (DimensionManager.LoadedDimension == mainDimension)
                yield return new WaitForSeconds(0.5f);
            else
                yield return DimensionManager.LoadDimension(mainDimension);
        }
        
        // play and wait for dialogue
        {
            // send cube
            cubeController.SetState(
                new List<GCubeState> {
                    new GCSGoTo(cubeController, dialoguePoint.Position),
                    new GCSDialogue(cubeController, dialogueText, 50f),
                    new GCSIdle(cubeController)
                });

            // wait for dialgue start
            while (DialogueManager.instance.IsDialoguePlaying == false)
                yield return null;

            // wait for select option
            while (Input.GetKey(KeyCode.Alpha1) == false)
                yield return null;


            float timer = timeForLastDialogue;
            while (timer > 0 && DialogueManager.instance.IsDialoguePlaying)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }

        // anim end
        {
            // turn on lights
            foreach (var light in lights)
            {
                light.intensity = 0;
                light.gameObject.SetActive(true);
            }

            // turn on background
            Color backgroundColor = new Color(1, 1, 1, 0);
            backround.color = backgroundColor;
            canvas.SetActive(true);

            float timer = 0;
            while (timer <= lightTintTime || timer <= whiteBackroundDelay + whiteBackroundTintInTime)
            {
                if (timer > whiteBackroundDelay)
                {
                    backgroundColor.a = Mathf.Clamp01((timer - whiteBackroundDelay) / whiteBackroundTintInTime);
                    backround.color = backgroundColor;
                }

                float lightsInts = Mathf.Clamp01(timer / lightTintTime) * lightsIntensity;
                foreach (var light in lights)
                    light.intensity = lightsInts;

                timer += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(whiteScreanDuration);
        }

        // return everything to normal
        {
            // set scene as it was in first time
            GameManager.SetFirstGameStats();

            // disable lights
            foreach (var light in lights)
                light.gameObject.SetActive(false);

            // return cube to start place
            cubeController.SetState(
                new List<GCubeState> {
                    new GCSGoTo(cubeController, dockingPoint.Position),
                    new GCSIdle(cubeController)
                });

            // enable movement
            playerController.FreezMovement = false;

            yield return new WaitForSeconds(1f);

            // hide background
            float timer = 0;
            Color backgroundColor = new Color(1, 1, 1, 1);
            while (timer <= whiteBackroundTintOutTime)
            {
                backgroundColor.a = 1 - timer / whiteBackroundTintOutTime;
                backround.color = backgroundColor;

                timer += Time.deltaTime;
                yield return null;
            }
            canvas.SetActive(false);
        }

        // wait for player recation
        {
            // wait for dialgue start
            while (DialogueManager.instance.IsDialoguePlaying == false)
                yield return null;

            yield return new WaitForSeconds(playerReactionDelay);
        }

        // Show EndScreen
        {
            canvas.SetActive(true);
            backToMenuButton.gameObject.SetActive(false);
            endPanel.SetActive(false);

            playerController.enabled = false;
            playerInteractions.enabled = false;

            float timer = 0;
            Color backgroundColor = new Color(0, 0, 0, 0);
            while (timer <= blackBackroundTintTime)
            {
                backgroundColor.a = timer / blackBackroundTintTime;
                backround.color = backgroundColor;

                timer += Time.deltaTime;
                yield return null;
            }
            backround.color = Color.black;


            yield return new WaitForSeconds(endPanelDealy);
            endPanel.SetActive(true);

            yield return new WaitForSeconds(blackButonShowDleay);
            GameManager.SetCursorState(false);
            backToMenuButton.onClick.AddListener(DimensionManager.BackToMenu);
            backToMenuButton.gameObject.SetActive(true);
        }
    }
}
