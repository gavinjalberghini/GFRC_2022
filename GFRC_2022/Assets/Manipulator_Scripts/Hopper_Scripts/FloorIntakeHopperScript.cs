using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FloorIntakeHopperScript : MonoBehaviour
{
    Keyboard_Controls kControls;
    PS4_Controls pControls;

    public GameObject cargo;//the prefab that gets instantiated, modified cargo prefab

    public GameObject launchableCargo;//unmodified prefab used with gravity feeder

    public Transform spawnPos_1;//where the cargo is spawned
    public Transform spawnPos_2;

    Basket_Gate_Script bG;
    Basket_Movement_Script bM;
    public GameObject basketGate;
    public GameObject basket;
    public Transform basketStorage_L;
    public Transform basketStorage_R;

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

        bG = basketGate.GetComponent<Basket_Gate_Script>();
        bM = basket.GetComponent<Basket_Movement_Script>();
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
                if (GameObject.Find("GravityBasket") != null)
                {
                    spawnedCargo_2.transform.position = basketStorage_R.position;
                    spawnedCargo_2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                    spawnedCargo_2.GetComponent<Rigidbody>().useGravity = true;
                    //Destroy(spawnedCargo_2);
                    //launchableCargo_2 = Instantiate(launchableCargo, basketStorage_R);
                    hasPickedUp_2 = false;
                    isLoaded = true;
                }
                else
                {
                    Destroy(spawnedCargo_2);
                    hasPickedUp_2 = false;
                    Debug.Log("cargo loaded");
                    isLoaded = true;
                }
            }
            else if(hasPickedUp_1 && !isLoaded)
            {
                if (GameObject.Find("GravityBasket") != null)
                {
                    spawnedCargo_1.transform.position = basketStorage_L.position;
                    spawnedCargo_1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                    spawnedCargo_1.GetComponent<Rigidbody>().useGravity = true;
                    Debug.Log("gravity");
                    //Destroy(spawnedCargo_1);
                    //launchableCargo_1 = Instantiate(launchableCargo, basketStorage_L);
                    hasPickedUp_1 = false;
                    isLoaded = true;
                }
                else
                {
                    Destroy(spawnedCargo_1);
                    hasPickedUp_1 = false;
                    Debug.Log("last cargo loaded");
                    isLoaded = true;
                }

  
            }
        }

        
        if ((isSpaceHeld || isLaunchPressed) && isLoaded && GameObject.Find("GravityBasket") != null && hasPickedUp_1)
        {
            Destroy(spawnedCargo_2);
            launchableCargo_2 = Instantiate(launchableCargo, basketStorage_R);
            isLoaded = false;
        }
        else if((isSpaceHeld || isLaunchPressed) && isLoaded && GameObject.Find("GravityBasket") != null && !hasPickedUp_1)
        {
            Destroy(spawnedCargo_1);
            launchableCargo_1 = Instantiate(launchableCargo, basketStorage_L);
            isLoaded = false;
        }
        else if ((isSpaceHeld || isLaunchPressed) && isLoaded)
        {
            isLoaded = false;
            Debug.Log("one cargo launched");
        }

        /*if((Gamepad.current.buttonNorth.ReadValue() > 0 || Keyboard.current.upArrowKey.ReadValue() > 0) && GameObject.Find("GravityBasket") != null)
        {
            bM.MoveUp();
        }
        if ((Gamepad.current.buttonSouth.ReadValue() > 0 || Keyboard.current.downArrowKey.ReadValue() > 0) && GameObject.Find("GravityBasket") != null)
        {
            bM.MoveDown();
        }*/


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
