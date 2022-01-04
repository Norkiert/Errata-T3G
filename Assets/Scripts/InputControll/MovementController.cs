using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;


public class MovementController : MonoBehaviour
{
    public enum Mods { None, Default, Sprint, Crouch }

    [Header("Default")]
    [SerializeField] [Min(1f)]
    private float defaultSpeed;

    [SerializeField] [Min(0.5f)]
    private float defaultHeight = 1.9f;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private bool canJump = true;

    [SerializeField] [ShowIf(nameof(canJump))] [Min(1f)]
    private float defaultJumpHeight;

    [Header("Sprint")]
    [SerializeField]
    private bool canSprint = true;

    [SerializeField] [ShowIf(nameof(canSprint))] [Min(1f)]
    private float sprintSpeed;

    [SerializeField] [ShowIf(EConditionOperator.And, nameof(canSprint), nameof(canJump))] [Min(1f)]
    private float sprintJumpHeight;

    [Header("Crouch")]
    [SerializeField]
    private bool canCrouch = true;

    [SerializeField] [ShowIf(nameof(canCrouch))] [Min(1f)]
    private float crouchSpeed;

    [SerializeField] [ShowIf(EConditionOperator.And, nameof(canCrouch), nameof(canJump))] [Min(1f)]
    private float crouchJumpHeight;

    [SerializeField] [ShowIf(nameof(canCrouch))] [Min(0.5f)]
    private float crouchHeight = 1.2f;


    [Header("Inputs")]
    [SerializeField] [ReadOnly] private bool inputJump = false;
    [SerializeField] [ReadOnly] private bool inputSprint = false;
    [SerializeField] [ReadOnly] private bool inputCrouch = false;
    [SerializeField] [ReadOnly] private float inputHorizontalMove = 0f;
    [SerializeField] [ReadOnly] private float inputVerticalMove = 0f;

    [Header("States")]
    [SerializeField] [ReadOnly] private bool isGrounded = false;
    [SerializeField] [ReadOnly] private Mods currentMod = Mods.None;
    [SerializeField] [ReadOnly] private float currentSpeed;
    [SerializeField] [ReadOnly] private float currentJumpHeight;


    private CharacterController controller;
    public Vector3 velocity;
    private Vector3 moveDirection;
    private DG.Tweening.Tweener hightModifier;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        SetMode(Mods.Default);
    }

    private void Update()
    {
        float radius = controller.radius * 0.9f;
        Vector3 groundCheck = controller.bounds.center - (controller.bounds.extents.y - radius + 0.2f) * Vector3.up;
        isGrounded = Physics.CheckSphere(groundCheck, radius, groundMask);

        GetInput();
        ChoseMode();
        MovePlayer();
    }

    private void SetMode(Mods mod)
    {
        if (currentMod == mod)
            return;

        switch (mod)
        {
            case Mods.Default:
                currentSpeed = defaultSpeed;
                currentJumpHeight = defaultJumpHeight;
                SetHeight(defaultHeight, 0.3f);
                break;
            case Mods.Sprint:
                currentSpeed = sprintSpeed;
                currentJumpHeight = sprintJumpHeight;
                SetHeight(defaultHeight, 0.3f);
                break;
            case Mods.Crouch:
                currentSpeed = crouchSpeed;
                currentJumpHeight = crouchJumpHeight;
                SetHeight(crouchHeight, 0.5f);
                break;
            default:
                currentSpeed = 0;
                currentJumpHeight = 0;
                SetHeight(defaultHeight);
                break;
        }

        currentMod = mod;
    }

    private void ChoseMode()
    {
        if (canSprint && inputSprint)
            SetMode(Mods.Sprint);
        else if (canCrouch && inputCrouch)
            SetMode(Mods.Crouch);
        else
            SetMode(Mods.Default);
    }

    private void SetHeight(float targetHeight, float speed)
    {
        hightModifier?.Kill();
        float time = Math.Abs(controller.height - targetHeight) * speed;
        hightModifier = DOVirtual.Float(controller.height, targetHeight, time, v => SetHeight(v, false));
    }
    private void SetHeight(float targetHeight, bool kill = true)
    {
        if(kill)
            hightModifier?.Kill();

        if (isGrounded)
            controller.Move(transform.up * (targetHeight - controller.height));

        controller.height = targetHeight;
    }


    private void GetInput()
    {
        // movement
        inputHorizontalMove = Input.GetAxisRaw("Horizontal");
        inputVerticalMove = Input.GetAxisRaw("Vertical");

        //sprint on/off
        inputSprint = Input.GetKey(KeyCode.LeftShift);

        //Crouching controller
        inputCrouch = Input.GetKey(KeyCode.LeftControl);

        //jumping
        inputJump = Input.GetKey(KeyCode.Space);
    }

    private void MovePlayer()
    {
        Vector3 newMoveDirection = transform.right * inputHorizontalMove + transform.forward * inputVerticalMove;
        float smothnes = (inputHorizontalMove > 0 || inputVerticalMove > 0 ? 10f : 100f) * Time.deltaTime;
        moveDirection = Vector3.Lerp(moveDirection, newMoveDirection, smothnes);
        controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);

        if (isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f;

            if (canJump && inputJump && velocity.y < 1f)
                velocity.y = Mathf.Sqrt(currentJumpHeight * -2f * Physics.gravity.y);
        }

        velocity += Physics.gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

