using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalScore : MonoBehaviour
{
    public GameObject Top;
    public GameObject Bottom;

    public int blueScoreTop;
    public int redScoreTop;
    public int blueScoreBot;
    public int redScoreBot;
    public int blueScore;
    public int redScore;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        blueScoreTop = Top.GetComponent<Hub_Behaviors>().BScore();
        blueScoreBot = Bottom.GetComponent<Hub_Behaviors>().BScore();
        redScoreTop = Top.GetComponent<Hub_Behaviors>().RScore();
        redScoreBot = Bottom.GetComponent<Hub_Behaviors>().RScore();
        blueScore = blueScoreTop + blueScoreBot;
        redScore = redScoreTop + redScoreBot;
    }
}
