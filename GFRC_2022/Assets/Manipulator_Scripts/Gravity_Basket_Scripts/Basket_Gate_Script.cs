using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Basket_Gate_Script : MonoBehaviour
{
    public float speed = 10f;
    Vector3 o = new Vector3(0, 0, -1);
    Vector3 c = new Vector3(0, 0, 1);
    // Start is called befhighore the first frame update
    void Start()
    {
        //Open();
    }

    // Update is called once per frame
    void Update()
    {

        if (Gamepad.current.buttonWest.ReadValue() > 0)
        {
            Open();
            c = new Vector3(0, 0, 1);
        }
        else
            Close();
    }

    public void Open()
    {
         transform.Translate(o * speed * Time.deltaTime);
    }

    public void Close()
    {
            transform.Translate(c * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "left_gate")
        {
            //Debug.Log("left boudary");
            o = new Vector3(0, 0, 0);
        }

        if(other.transform.tag == "right_gate")
        {
            //Debug.Log("right boundary");
            c = new Vector3(0, 0, 0);
            o = new Vector3(0, 0, -1);

        }
    }
}
