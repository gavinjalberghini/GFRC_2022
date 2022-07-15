using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Basket_Movement_Script : MonoBehaviour
{
    public float speed = 5.0f;
    public float upperBound = 0.8f;
    public float lowerBound = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        //MoveUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.buttonNorth.ReadValue() > 0)
            MoveUp();
        else
            MoveDown();

    }

    public void MoveUp()
    {
        if(gameObject.transform.position.y < upperBound)
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    
    public void MoveDown()
    {
        if(gameObject.transform.position.y > lowerBound)
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
