using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanIntakeHopper : MonoBehaviour
{
    Keyboard_Controls kControls;
    PS4_Controls pControls;

    public GameObject cargo;//the prefab that gets instantiated, modified cargo prefab

    public GameObject launchableCargo;//unmodified prefab used with gravity feeder

    public Transform spawnPos_1;//where the cargo is spawned
    public Transform spawnPos_2;

    public Transform basketStorage_1;
    public Transform basketStorage_2;

    private GameObject spawnedCargo_1;//instantiated object for scripting
    private GameObject spawnedCargo_2;

    private GameObject launchableCargo_1;
    private GameObject launchableCargo_2;

    bool hasPickedUp_1 = false;//logic variables
    bool hasPickedUp_2 = false;
    private int pickupCounter = 0;
    private bool isLoaded = false;
    // Start is called before the first frame update
    void Awake()
    {
        pControls = new PS4_Controls();
        kControls = new Keyboard_Controls();
    }


    // Update is called once per frame
    void Update()
    {
        bool isRHeld = kControls.Gameplay.RKey.ReadValue<float>() > 0.1f;
        bool isSpaceHeld = kControls.Gameplay.Spacebar.ReadValue<float>() > 0.1f;

        bool isLoadPressed = pControls.Gameplay.Load.ReadValue<float>() > 0.1f;
        bool isLaunchPressed = pControls.Gameplay.Launch.ReadValue<float>() > 0.1f;



        if (hasPickedUp_1)
        {

            spawnedCargo_1.transform.position = spawnPos_1.position;
        }

        if (hasPickedUp_2)
        {

            spawnedCargo_2.transform.position = spawnPos_2.position;
        }

        if ((isRHeld || Gamepad.current.buttonEast.wasPressedThisFrame) && pickupCounter > 0 && GameObject.Find("GravityBasket") != null)
        {
            LoadCargoGB();
        }
        else if ((isRHeld || isLoadPressed) && pickupCounter > 0 && !isLoaded)
        {
            LoadCargo();
        }

        //methods of 'launching' cargo
        if ((isSpaceHeld || isLaunchPressed) && isLoaded && GameObject.Find("GravityBasket") == null)
        {
            isLoaded = false;
            Debug.Log("one cargo launched");
        }


    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("cargo") && !hasPickedUp_2)
        {
            pickupCounter++;

            if (pickupCounter == 1)
            {
                spawnedCargo_1 = Instantiate(cargo, spawnPos_1.position, spawnPos_1.rotation);
                hasPickedUp_1 = true;
                Destroy(collision.gameObject);
            }

            if (pickupCounter == 2)
            {
                spawnedCargo_2 = Instantiate(cargo, spawnPos_2.position, spawnPos_2.rotation);
                hasPickedUp_2 = true;
                Destroy(collision.gameObject);
            }
        }
    }

    public void LoadCargo()
    {
        pickupCounter--;

        if (hasPickedUp_2)
        {
            Destroy(spawnedCargo_2);
            hasPickedUp_2 = false;
            //Debug.Log("cargo loaded");
            isLoaded = true;
        }
        else if (hasPickedUp_1 && !isLoaded)
        {
            Destroy(spawnedCargo_1);
            hasPickedUp_1 = false;
            //Debug.Log("last cargo loaded");
            isLoaded = true;
        }
    }

    public void LoadCargoGB()
    {
        pickupCounter--;

        if (hasPickedUp_2)
        {
            //spawns cargo prefab into gravity bucket
            Destroy(spawnedCargo_2);
            Vector3 spawnPos1 = new Vector3(basketStorage_2.transform.position.x, basketStorage_2.transform.position.y, basketStorage_2.position.z);
            launchableCargo_2 = Instantiate(launchableCargo);
            launchableCargo_2.transform.position = spawnPos1;
            hasPickedUp_2 = false;
        }
        else if (hasPickedUp_1)
        {
            //spawns cargo prefab into gravity bucket
            Destroy(spawnedCargo_1);
            Vector3 spawnPos = new Vector3(basketStorage_1.transform.position.x, basketStorage_1.transform.position.y, basketStorage_1.position.z);
            launchableCargo_1 = Instantiate(launchableCargo);
            launchableCargo_1.transform.position = spawnPos;
            hasPickedUp_1 = false;
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
