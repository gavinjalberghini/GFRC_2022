using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple_Rotate_Script : MonoBehaviour
{
    public float rotaSpeed = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(transform.rotation.y);
        //allow rotation

        if (Gamepad.current.rightStick.x.ReadValue() < 0)
        {
            transform.Rotate(Vector3.down * Time.deltaTime * rotaSpeed);
        }

        if (Gamepad.current.rightStick.x.ReadValue() > 0)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotaSpeed);
        }

  
    }
}
