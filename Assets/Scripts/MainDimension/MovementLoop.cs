using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLoop : MonoBehaviour
{
    [Header("Radius of player loop")]
    [SerializeField] private float radius = 190f;

    private PlayerController playerControll;

    private void Start()
    {
        playerControll = FindObjectOfType<PlayerController>();
        if (playerControll == null)
        {
            Debug.LogWarning($"{name}: dont found player!");
            enabled = false;
        }
    }

    private void Update()
    {
        float distance = Mathf.Sqrt(Mathf.Pow(playerControll.transform.position.x, 2) + Mathf.Pow(playerControll.transform.position.z, 2));
        if (distance>radius)
        {
            float newX = playerControll.transform.position.x > 0 ? -playerControll.transform.position.x + 2 : -playerControll.transform.position.x - 2;
            float newZ = playerControll.transform.position.z > 0 ? -playerControll.transform.position.z + 2 : -playerControll.transform.position.z - 2;
            playerControll.SetPosition(new Vector3(newX, playerControll.transform.position.y, newZ));
        }
    }
}
