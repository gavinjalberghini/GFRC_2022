using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TMShooter : MonoBehaviour
{
    Keyboard_Controls kControls;
    PS4_Controls pControls;

    public GameObject cargo;
    GameObject thrownCargo;
    public float force = 100f;
    public Transform pitch;
    public Transform twist;

    // Start is called before the first frame update
    void Awake()
    {
        pControls = new PS4_Controls();
        kControls = new Keyboard_Controls();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = new Vector3(0, (-2 * pitch.rotation.x), 0);

        if (Keyboard.current.spaceKey.wasPressedThisFrame || Gamepad.current.rightTrigger.wasPressedThisFrame)
        {
            thrownCargo = Instantiate(cargo, transform.position, transform.rotation);

            thrownCargo.GetComponent<Rigidbody>().AddForce((twist.forward + v) * force);

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
