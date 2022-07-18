using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanger_Behaviors : MonoBehaviour
{
    public GameObject R1;
    public GameObject R2;
    public GameObject R3;
    public GameObject R4;

    bool R1Hanging;
    bool R2Hanging;
    bool R3Hanging;
    bool R4Hanging;

    public int score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        R1Hanging = R1.GetComponent<Railing_Behaviors>().robotHanging;
        R2Hanging = R2.GetComponent<Railing_Behaviors>().robotHanging;
        R3Hanging = R3.GetComponent<Railing_Behaviors>().robotHanging;
        R4Hanging = R4.GetComponent<Railing_Behaviors>().robotHanging;
        if (R1Hanging)
        {
            score = 4;
        }
        else if (R2Hanging)
        {
            score = 6;
        }
        else if (R3Hanging)
        {
            score = 10;
        }
        else if (R4Hanging)
        {
            score = 15;
        }
        else 
        {
            score = 0;
        }
    }
}
