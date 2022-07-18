using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railing_Behaviors : MonoBehaviour
{
    public bool robotHanging;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider robo) 
    {
        if (robo.gameObject.layer == 8)
        {
            robotHanging = true;
        }
    }

    void OnTriggerExit(Collider robo)
    {
        if (robo.gameObject.layer == 8)
        {
            robotHanging = false;
        }
    }
}
