using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TMShooterRotate : MonoBehaviour
{
    Keyboard_Controls kControls;
    PS4_Controls pControls;
    private int rotaAmt;

    public float rotaSpeed = 25f;
    // Start is called before the first frame update
    void Awake()
    {
        pControls = new PS4_Controls();
        kControls = new Keyboard_Controls();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseWheel = Mouse.current.scroll.ReadValue();

        if (((mouseWheel.y > 0) || Gamepad.current.rightStick.y.ReadValue() > 0) && (transform.rotation.x <= 0.1) && (transform.rotation.x > -0.45))
        {
            transform.Rotate(Vector3.left * Time.deltaTime * rotaSpeed);
        }
        else if ((mouseWheel.y < 0 || Gamepad.current.rightStick.y.ReadValue() < 0) && ((transform.rotation.x <= 0.1) && (transform.rotation.x > -0.45)))
        {
            transform.Rotate(Vector3.right * Time.deltaTime * rotaSpeed);
        }
        else//stop excess rotation
        {
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);


            if(transform.rotation.x > 0.02)
            {
                transform.Rotate(Vector3.left * Time.deltaTime * rotaSpeed);
            }
            if (transform.rotation.x < -0.45)
            {
                transform.Rotate(Vector3.right * Time.deltaTime * rotaSpeed);
            }
        }



    }
    private void OnEnable()
    {
        pControls.Gameplay.Enable();
        kControls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        pControls.Gameplay.Disable();
        kControls.Gameplay.Disable();
    }
}
