using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromLasers : MonoBehaviour
{
    private SpriteRenderer planeSR;
    private HubPlayerHandler playerHandler;

    [SerializeField] private SpriteRenderer portalBlackScreen;


    private void Start()
    {
        GameObject plane = GameObject.Find("PlayerLaserPlane");
        if (plane == null)
        {
            Debug.LogWarning("Cant found LaserPlane");
            return;
        }

        planeSR = plane.GetComponent<SpriteRenderer>();
        playerHandler = FindObjectOfType<HubPlayerHandler>();

        if (SaveManager.IsLevelFinished(Dimension.Laser))
        {
            DsiableBlack();
            return;
        }

        if (playerHandler != null)
        {
            playerHandler.OnChange += SwitchPlanes;
            SwitchPlanes();
        }
        else
            Debug.LogError("doun found playerHandler");
    }

    private void OnDisable()
    {
        playerHandler.OnChange -= SwitchPlanes;
        DsiableBlack();
    }

    private void SwitchPlanes()
    {
        if (playerHandler.IsPlayerInHub)
        {
            planeSR.enabled = false;
            portalBlackScreen.enabled = true;
        }
        else
        {
            planeSR.enabled = true;
            portalBlackScreen.enabled = false;
        }
    }

    private void DsiableBlack()
    {
        if (portalBlackScreen != null)
            portalBlackScreen.enabled = false;

        if (planeSR != null)
            planeSR.enabled = false;
    }
}
