using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanFeederIntake : MonoBehaviour
{
    public GameObject cargo;
    public Transform spawnPos_1;
    private GameObject spawnedCargo_1;
    bool hasPickedUp_1 = false;
    //public Transform spawnTwo;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hasPickedUp_1)
        {
            spawnedCargo_1.transform.position = spawnPos_1.position;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("cargo"))
        {
            Destroy(collision.gameObject);

            spawnedCargo_1 = Instantiate(cargo, spawnPos_1.position, spawnPos_1.rotation);
            hasPickedUp_1 = true;
        }
    }
}
