using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TMRotate : MonoBehaviour
{
    Keyboard_Controls kControls;
    PS4_Controls pControls;

    public float rotaSpeed = 10f;
    // Start is called before the first frame update
    void Awake()
    {
        pControls = new PS4_Controls();
        kControls = new Keyboard_Controls();
    }

    // Update is called once per frame
    void Update()
    {

        bool isQHeld = kControls.Gameplay.QKey.ReadValue<float>() > 0.1f;
        bool isEHeld = kControls.Gameplay.EKey.ReadValue<float>() > 0.1f;


        if(isQHeld || Gamepad.current.rightStick.x.ReadValue() < 0)
        {
            transform.Rotate(Vector3.down * Time.deltaTime * rotaSpeed);
        }

        if (isEHeld || Gamepad.current.rightStick.x.ReadValue() > 0)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotaSpeed);
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
