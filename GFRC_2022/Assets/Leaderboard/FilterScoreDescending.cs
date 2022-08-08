using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FilterScoreDescending : MonoBehaviour
{
    public Management m;
    public void SortByScoreDescending()
    {
        m.FilterDescending();
        //Array.Reverse([array of scores])

    }
}
