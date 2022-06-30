using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swerve_drive : MonoBehaviour
{

    public Rigidbody rb;
    public float MaxVel;
    public float acceleration;
    public float angle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        angle = transform.localRotation.y;

        if (Input.GetKeyDown("W")) { rb.velocity = new Vector3(MaxVel*Mathf.cos(angle), 0f, MaxVel*Mathf.sin(angle)); }
    }
}
