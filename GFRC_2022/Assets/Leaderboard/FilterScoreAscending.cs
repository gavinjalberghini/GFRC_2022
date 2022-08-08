using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FilterScoreAscending : MonoBehaviour
{
    public Management m;
    public void SortByScoreAscending()
    {
        m.FilterAscending();
        //iterate through a loop of all recorded scores and sort low -> high
        //Array.Sort([array of scores])

    }
}
