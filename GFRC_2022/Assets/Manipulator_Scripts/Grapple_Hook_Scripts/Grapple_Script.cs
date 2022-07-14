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

        if(Gamepad.current.leftTrigger.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartGrapple();
        }


        if (Gamepad.current.leftTrigger.wasReleasedThisFrame || Mouse.current.leftButton.wasReleasedThisFrame)
        {
                StopGrapple();
        }

        if (Gamepad.current.leftTrigger.ReadValue() > 0.1f || Mouse.current.leftButton.ReadValue() > 0.1f)
        {
            WhileGrappling();
        }

       

    }

    private void LateUpdate()
    {
        DrawGrapple();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(grappleTip.position, grappleTip.forward, out hit, maxDistance, WhatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = robot.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(robot.transform.position, grapplePoint);

            joint.maxDistance = 0.8f;
            joint.minDistance = 0.2f;


            lr.positionCount = 2;
        }
    }

    void WhileGrappling()
    {
        if (Gamepad.current.leftShoulder.ReadValue() > 0f || Mouse.current.rightButton.ReadValue() > 0f)
        {
            robot.GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePosition;
            //Debug.Log("moving");
            //NOTE: AdJUST VALUES IF MASS OF ROBOT IS DIFFERENT
            joint.spring = 1000.0f;//force robot is moved towards point
            joint.damper = 7.5f;//force acting against spring
            joint.massScale = 4.5f;
        }
        else if(Gamepad.current.leftShoulder.wasReleasedThisFrame || Mouse.current.rightButton.wasReleasedThisFrame)
        {
            robot.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //Debug.Log("frozen");

        }
    }

    void DrawGrapple()
    {
        if(!joint)
        {
            return;
        }

        lr.SetPosition(0, grappleTip.position);
        lr.SetPosition(1, grapplePoint);

    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
        //Debug.Log("unfrozen");
        robot.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

}
