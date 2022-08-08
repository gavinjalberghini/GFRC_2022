using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FilterUsernameZA : MonoBehaviour
{
    public Management m;
    public void SortByUsernameZA()
    {
        m.FilterZA();
        //iterate through a loop of all recorded usernames and backwards alpha sort

        //Array.Sort(usernames, (x, y) => String.Compare(y.Username, x.Username));
        //attempted reverse alpha
    }
}
