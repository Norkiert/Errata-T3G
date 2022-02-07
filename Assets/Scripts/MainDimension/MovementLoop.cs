using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLoop : MonoBehaviour
{
    [Header("Radius of player loop")]
    [SerializeField] private float radius = 190f;

    private Transform playerPosition;
    private PlayerController playerControll;
    void Start()
    {
        playerPosition = GameObject.Find("Player").transform;
        playerControll = FindObjectOfType<PlayerController>();
    }
    void Update()
    {
        float distance = Mathf.Sqrt(Mathf.Pow(playerPosition.position.x, 2) + Mathf.Pow(playerPosition.position.z, 2));
        if (distance>radius)
        {
            float newX = playerPosition.position.x > 0 ? -playerPosition.position.x + 2 : -playerPosition.position.x - 2;
            float newZ = playerPosition.position.z > 0 ? -playerPosition.position.z + 2 : -playerPosition.position.z - 2;
            playerControll.SetPosition(new Vector3(newX, playerPosition.position.y, newZ));
        }
    }
}
