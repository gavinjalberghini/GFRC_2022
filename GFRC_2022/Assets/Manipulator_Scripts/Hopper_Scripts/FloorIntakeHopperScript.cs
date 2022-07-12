using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorIntakeHopperScript : MonoBehaviour
{
    Keyboard_Controls kControls;
    PS4_Controls pControls;

    public GameObject cargo;//the prefab that gets instantiated, modified cargo prefab

    public Transform spawnPos_1;//where the cargo is spawned
    public Transform spawnPos_2;

    private GameObject spawnedCargo_1;//instantiated object for scripting
    private GameObject spawnedCargo_2;

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

        if(hasPickedUp_2)
        {
            spawnedCargo_2.transform.position = spawnPos_2.position;
        }

        if((isRHeld || isLoadPressed)&& pickupCounter > 0 && !isLoaded)
        {
            pickupCounter--;

            if(hasPickedUp_2)
            {
                Destroy(spawnedCargo_2);
                hasPickedUp_2 = false;
                Debug.Log("cargo loaded");
                isLoaded = true;
            }
            else if(hasPickedUp_1 && !isLoaded)
            {
                Destroy(spawnedCargo_1);
                hasPickedUp_1 = false;
                Debug.Log("last cargo loaded");
                isLoaded = true;
            }
        }

        if ((isSpaceHeld || isLaunchPressed) && isLoaded)
        {
            isLoaded = false;
            Debug.Log("one cargo launched");
        }


    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("cargo") && !hasPickedUp_2)
        {
            pickupCounter++;

            if(pickupCounter == 1)
            {
                spawnedCargo_1 = Instantiate(cargo, spawnPos_1.position, spawnPos_1.rotation);
                hasPickedUp_1 = true;
                Destroy(collision.gameObject);
            }

            if(pickupCounter == 2)
            {
                spawnedCargo_2 = Instantiate(cargo, spawnPos_2.position, spawnPos_2.rotation);
                hasPickedUp_2 = true;
                Destroy(collision.gameObject);
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
