using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDrive : MonoBehaviour
{
    public float speed = 7f;
    private float rotaSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        #region Forward Movement
        //move forward
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            if (Input.GetKey(KeyCode.A))//move forward and slowly move left, angling left
            {
                transform.Rotate(0f, -.03f, 0f * rotaSpeed);
            }
            if (Input.GetKey(KeyCode.D))//move forward and slowly move right, angling right
            {
                transform.Rotate(0f, .03f, 0f * rotaSpeed);
            }
        }
    

        #endregion
        #region Backwards Movement
        //move backwards
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);

            if (Input.GetKey(KeyCode.A))//move backwards and slowly move left, angling towards right
            {
                transform.Rotate(0f, .03f, 0f * rotaSpeed);
            }
            if (Input.GetKey(KeyCode.D))//move backwards and slowly move right, angling towards left
            {
                transform.Rotate(0f, -.03f, 0f * rotaSpeed);
            }
        }

        ////NOTE: backwards y val rotations are opposite of forwards y val rotations because the tank is reversing
        #endregion

        //only uncomment if each side of wheels is controllable
        #region Rotation
        //rotate left
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0f, -.04f, 0f * rotaSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0f, .04f, 0f * rotaSpeed);
        }
        #endregion
    }
}
