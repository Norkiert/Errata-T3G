using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

public class CubeController : MonoBehaviour
{
    public enum CubeStates {None, Idle, Follow, Questions}

    [SerializeField, ReadOnly] private CubeStates actualState = CubeStates.None;

    [Header("Idle")]
    [SerializeField] private float flyingUpTime = 1f;
    [SerializeField] private float flyingUpDistance = 1f;

    [Header("Movement")]
    [SerializeField] private Vector3 followOffset = new Vector3(-1, 1, 0);
    [SerializeField] private float flyingSpeed = 5f;

    private PlayerController player;
    private Vector3 idlePointPosition;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        idlePointPosition = transform.position;
        SwitchState(CubeStates.Idle);
    }

    private void SwitchState(CubeStates newState)
    {
        if (newState == actualState)
            return;
        switch(newState)
        {
            case CubeStates.Idle:
                {
                    GoToIdlePoint();
                    StartCoroutine(IdleStateBehavior());
                    break;
                }
            case CubeStates.Follow:
                {
                    StartCoroutine(FollowPlayer());
                    break;
                }
            case CubeStates.Questions:
                {
                    //QuestionsCode
                    break;
                }
            default:
                {
                    Debug.Log("State switch error");
                    break;
                }
        }
        actualState = newState;
    }
    private void GoToIdlePoint()
    {
        transform.position = idlePointPosition;
    }

    //Function called when player get into trigger field of idle point
    public void SetNewIdlePoint(Vector3 newPosition)
    {
        StopAllCoroutines();
        idlePointPosition = newPosition;
        SwitchState(CubeStates.Idle);
    }

    //Function called when player exit trigger field of idle point
    public void StartFollowing()
    {
        DOTween.PauseAll();
        StopAllCoroutines();
        SwitchState(CubeStates.Follow);
    }

    public IEnumerator GoToPoint(Vector3 targetPosition)
    {
        transform.LookAt(targetPosition);
        while(transform.position!=targetPosition)
        {
            transform.position += Vector3.forward * flyingSpeed;
            yield return null;
        }
        StopCoroutine(GoToPoint(Vector3.zero));
    }

    //Idle Coroutine
    private IEnumerator IdleStateBehavior()
    {
        bool flyUp = true;
        while(true)
        {
            if (flyUp)
            {
                transform.DOMoveY(transform.position.y + flyingUpDistance, flyingUpTime);
                transform.DORotate(new Vector3((Random.value * 1000) % 360, (Random.value*1000)%360 , (Random.value * 1000) % 360), flyingUpTime);
                yield return new WaitForSeconds(flyingUpTime);
                flyUp = false;
            }
            else
            {
                transform.DOMoveY(transform.position.y - flyingUpDistance, flyingUpTime);
                transform.DORotate(new Vector3((Random.value * 1000) % 360, (Random.value * 1000) % 360, (Random.value * 1000) % 360), flyingUpTime);
                yield return new WaitForSeconds(flyingUpTime);
                flyUp = true;
            }
            yield return null;
        }
    }
    ///Follow Coroutine
    private IEnumerator FollowPlayer()
    {
        while(true)
        {
            transform.position = player.transform.position + followOffset;
            yield return null;
        }
    }
}
