using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovementController : MonoBehaviour
{
    private Rigidbody rB;
    private Transform model;
    private bool inSprint;
    private bool isCrouching = false;

    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private LayerMask groundMask;

    void Start()
    {
        rB = GetComponent<Rigidbody>();
        model = GetComponent<Transform>();
    }

    void Update()
    {
        float horizontalMove;
        float verticalMove;
        float currentHeight = transform.localScale.y;
        bool isOnGround = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - currentHeight, transform.position.z), 0.5f, groundMask);

        if(isCrouching)
        {
            horizontalMove = Input.GetAxis("Horizontal") * crouchSpeed;
            verticalMove = Input.GetAxis("Vertical") * crouchSpeed;
        }
        else if (inSprint)
        {
            horizontalMove = Input.GetAxis("Horizontal") * sprintSpeed;
            verticalMove = Input.GetAxis("Vertical") * sprintSpeed;
        }
        else
        {
            horizontalMove = Input.GetAxis("Horizontal") * speed;
            verticalMove = Input.GetAxis("Vertical") * speed;
        }

        Vector3 MovePosition = transform.right * horizontalMove + transform.forward * verticalMove;
        Vector3 NewMovePosition = new Vector3(MovePosition.x, rB.velocity.y, MovePosition.z);
        rB.velocity = NewMovePosition;

        //sprint on/off
        if (Input.GetKey(KeyCode.LeftShift) && isOnGround)
        {
            inSprint = true;
        }
        else
        {
            inSprint = false;
        }

        //Crouching controller
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isOnGround)
            {
                if (isCrouching)
                {
                    isCrouching = false;
                    model.DOScaleY(1.7f, 0.25f);
                }
                else
                {
                    isCrouching = true;
                    model.DOScaleY(1f, 0.3f);
                }
            }
        }

        //jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround)
            {
                    if (isCrouching)
                    {
                        rB.velocity = new Vector3(rB.velocity.x, jumpForce, rB.velocity.z);
                        model.DOScaleY(1.7f, 0.01f);
                        isCrouching = false;
                    }
                    else
                    {
                        rB.velocity = new Vector3(rB.velocity.x, jumpForce, rB.velocity.z);
                    }
            }
        }
    }
}

