using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour
{
    public void ToMenuScene()
    {
        print("returning to menu");
        //use scene management to switch to main menu UI
        //SceneManagement.LoadScene([main menu UI]);
    }
}
