using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalHandler : PortalTraveller
{
    public float rotationSmoothTime = 0.1f;

    MovementController m_controller;
    CameraController c_controller;

    float yaw;
    float smoothYaw;
    float yawSmoothV;
    Vector3 velocity;

    private void Start()
    {
        yaw = transform.eulerAngles.y;
        smoothYaw = yaw;

        m_controller = GetComponent<MovementController>();
        c_controller = GetComponent<CameraController>();
    }

    private void Update()
    {
        velocity = m_controller.velocity;

        yaw += Input.GetAxisRaw("Mouse X") * c_controller.sensitivity;
        smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);
    }

    public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle(smoothYaw, eulerRot.y);
        yaw += delta;
        smoothYaw += delta;
        transform.eulerAngles = Vector3.up * smoothYaw;
        velocity = toPortal.TransformVector(fromPortal.InverseTransformVector(velocity));
        Physics.SyncTransforms();
    }
}
