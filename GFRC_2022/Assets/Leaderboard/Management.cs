using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Management : MonoBehaviour
{
    public int amountOfEntries;

    [SerializeField] private List<GameObject> inputList;

    void Start()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("InputLine"))
        {
            inputList.Add(obj.gameObject);
        }

    }

    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //inputList[0].transform.GetChild(0).GetComponent<Text>().text = "it worked again";
        }
    }



    public void FilterDescending()
    {
        print("sorting by score from highest to lowest");
        List<int> tempList = new List<int>();//use to compare all highscores

        for (int i = 0; i < inputList.Count; i++)
        {
            int highscore = int.Parse(inputList[i].transform.GetChild(1).GetComponent<Text>().text);
            tempList.Add(highscore);
            tempList.Sort((x, y) => y.CompareTo(x));
        }
        foreach (int x in tempList)
        {
            print(x);
        }
    }

    public void FilterAscending()
    {
        print("sorting by score from lowest to highest");
        List<int> tempList = new List<int>();//use to compare all highscores

        for (int i = 0; i < inputList.Count; i++)
        {
            int highscore = int.Parse(inputList[i].transform.GetChild(1).GetComponent<Text>().text);
            tempList.Add(highscore);
            tempList.Sort();
        }
        foreach (int x in tempList)
        {
            print(x);
        }
    }

    public void FilterAZ()
    {
        print("sorting by username from A to Z");
        List<string> tempList = new List<string>();//use to compare usernames

        for (int i = 0; i < inputList.Count; i++)
        {
            string username = inputList[i].transform.GetChild(0).GetComponent<Text>().text;
            tempList.Add(username);
            tempList.Sort();
        }
        foreach (string x in tempList)
        {
            print(x);
        }
    }

    public void FilterZA()
    {
        print("sorting by username from Z to A");
        List<string> tempList = new List<string>();//use to compare usernames

        for (int i = 0; i < inputList.Count; i++)
        {
            string username = inputList[i].transform.GetChild(0).GetComponent<Text>().text;
            tempList.Add(username);
            tempList.Reverse();
        }
        foreach (string x in tempList)
        {
            print(x);
        }
    }

}
