using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FilterUsernameAZ : MonoBehaviour
{
    public Management m;
    public void SortByUsernameAZ()
    {
        m.FilterAZ();
        //iterate through a loop of all recorded usernames and alpha sort

        //Array.Sort(usernames, (x, y) => String.Compare(x.Username, y.Username));
    }



}
