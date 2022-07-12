using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMovement : MonoBehaviour
{

    Keyboard_Controls kControls;
    PS4_Controls pControls;

    public float speed = 7f;
    private float rotaSpeed = 2f;
    // Start is called before the first frame update
    void Awake()
    {
        pControls = new PS4_Controls();
        kControls = new Keyboard_Controls();
    }

    // Update is called once per frame
    void Update()
    {


        bool isWHeld = kControls.Gameplay.WKey.ReadValue<float>() > 0.1f;
        bool isAHeld = kControls.Gameplay.AKey.ReadValue<float>() > 0.1f;
        bool isSHeld = kControls.Gameplay.SKey.ReadValue<float>() > 0.1f;
        bool isDHeld = kControls.Gameplay.DKey.ReadValue<float>() > 0.1f;

        

        //move forward
        if (isWHeld || Gamepad.current.leftStick.y.ReadValue() > 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            if (isAHeld || Gamepad.current.leftStick.x.ReadValue() < 0)//move forward and slowly move left, angling left
            {
                transform.Rotate(0f, -.05f, 0f * rotaSpeed);
            }
            if (isDHeld || Gamepad.current.leftStick.x.ReadValue() > 0)//move forward and slowly move right, angling right
            {
                transform.Rotate(0f, .05f, 0f * rotaSpeed);
            }
        }

        //move backwards
        if (isSHeld || Gamepad.current.leftStick.y.ReadValue() < 0)
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);

            if (isAHeld || Gamepad.current.leftStick.x.ReadValue() < 0)//move backwards and slowly move left, angling towards right
            {
                transform.Rotate(0f, .05f, 0f * rotaSpeed);
            }
            if (isDHeld || Gamepad.current.leftStick.x.ReadValue() > 0)//move backwards and slowly move right, angling towards left
            {
                transform.Rotate(0f, -.05f, 0f * rotaSpeed);
            }
        }


        //rotate left
        if (isAHeld || Gamepad.current.leftStick.x.ReadValue() < 0)
        {
            transform.Rotate(0f, -.06f, 0f * rotaSpeed);
        }
        if (isDHeld || Gamepad.current.leftStick.x.ReadValue() > 0)
        {
            transform.Rotate(0f, .06f, 0f * rotaSpeed);
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
