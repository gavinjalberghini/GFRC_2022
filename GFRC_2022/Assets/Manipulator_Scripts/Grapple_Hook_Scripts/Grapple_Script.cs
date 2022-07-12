using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple_Script : MonoBehaviour
{
    private LineRenderer lr;
    Vector3 grapplePoint;
    public LayerMask WhatIsGrappleable;
    public Transform grappleTip;
    public float maxDistance = 75f;
    public GameObject robot;
    private SpringJoint joint;

    // Start is called before the first frame update
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    // Update is called once per frame
    void Update()
    {

        if(Gamepad.current.leftTrigger.wasPressedThisFrame)
        {
            this.gameObject.GetComponent<LineRenderer>().enabled = true;
            StartGrapple();
        }

        if(Gamepad.current.leftTrigger.wasReleasedThisFrame)
        {
            this.gameObject.GetComponent<LineRenderer>().enabled = false;
            StopGrapple();
        }

    }

    private void LateUpdate()
    {
        DrawGrapple();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(grappleTip.position, grappleTip.forward, out hit, maxDistance))
        {
            grapplePoint = hit.point;
            joint = robot.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(robot.transform.position, grapplePoint);

            joint.maxDistance = 0.8f;
            joint.minDistance = 0.2f;

            joint.spring = 8.0f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            robot.GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void DrawGrapple()
    {

        lr.SetPosition(0, grappleTip.position);
        lr.SetPosition(1, grapplePoint);

    }

    void StopGrapple()
    {
        Destroy(joint);
    }
}
