using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLoop : MonoBehaviour
{
    [Header("Borders for player (on X)")]
    [SerializeField] private float rightBorder = 220f;
    [SerializeField] private float leftBorder = -220f;

    [Header("Borders for player (on Z)")]
    [SerializeField] private float topBorder = 220f;
    [SerializeField] private float bottomBorder = -240f;

    private Transform playerPosition;
    private PlayerController playerControll;
    void Start()
    {
        playerPosition = GameObject.Find("Player").transform;
        playerControll = FindObjectOfType<PlayerController>();
    }
    void Update()
    {
        if (playerPosition.position.x > rightBorder)
        {
            playerControll.SetPosition(new Vector3(leftBorder + 2, playerPosition.position.y, -playerPosition.position.z));
        }
        if (playerPosition.position.x < leftBorder)
        {
            playerControll.SetPosition(new Vector3(rightBorder - 2, playerPosition.position.y, -playerPosition.position.z));
        }
        if (playerPosition.position.z > topBorder)
        {
            playerControll.SetPosition(new Vector3(-playerPosition.position.x, playerPosition.position.y, bottomBorder+2));
        }
        if (playerPosition.position.z < bottomBorder)
        {
            playerControll.SetPosition(new Vector3(-playerPosition.position.x, playerPosition.position.y, topBorder-2));
        }
    }
}
