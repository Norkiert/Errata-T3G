using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Rigidbody Rb;
    private bool InSprint;
    private bool isOnGround;

    public float Speed;
    public float SprintSpeed;
    public float JumpForce;
    public LayerMask GroundMask;

    float HorizontalMove;
    float VerticalMove;
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        isOnGround = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1.7f, transform.position.z), 0.5f, GroundMask);

        if (InSprint)
        {
            HorizontalMove = Input.GetAxis("Horizontal") * SprintSpeed;
            VerticalMove = Input.GetAxis("Vertical") * SprintSpeed;
        }
        else
        {
            HorizontalMove = Input.GetAxis("Horizontal") * Speed;
            VerticalMove = Input.GetAxis("Vertical") * Speed;
        }

        Vector3 MovePosition = transform.right * HorizontalMove + transform.forward * VerticalMove;
        Vector3 NewMovePosition = new Vector3(MovePosition.x, Rb.velocity.y, MovePosition.z);
        Rb.velocity = NewMovePosition;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround)
            {
                Rb.velocity = new Vector3(Rb.velocity.x, JumpForce, Rb.velocity.z);
            }
        }

        if (Input.GetKey(KeyCode.LeftShift) && isOnGround)
        {
            InSprint = true;
        }
        else
        {
            InSprint = false;
        }
    }
}

