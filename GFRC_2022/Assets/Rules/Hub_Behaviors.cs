using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub_Behaviors : MonoBehaviour
{
    public int blueScore;
    public int redScore;
    public int autoScoreRange;
    public int scoreMultiplier;
    public Transform SpawnPoint1;
    public Transform SpawnPoint2;
    public Transform SpawnPoint3;
    public Transform SpawnPoint4;

    public GameObject BlueCargo;
    public GameObject RedCargo;

    // Start is called before the first frame update
    void Start()
    {
        blueScore = Random.Range(0, autoScoreRange);
        redScore = Random.Range(0, autoScoreRange);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider cargo)
    {
        if (cargo.gameObject.layer == 6 || cargo.gameObject.layer == 7)
        { Score(cargo.gameObject); }
    }

    void Score(GameObject cargo)
    {
        int shoot = Random.Range(1, 4);
        Transform shot = SpawnPoint1;
        Destroy(cargo);
        switch (shoot)
        {
            case 1:
                shot = SpawnPoint1;
                break;
            case 2:
                shot = SpawnPoint2;
                break;
            case 3:
                shot = SpawnPoint3;
                break;
            case 4:
                shot = SpawnPoint4;
                break;
        }
        if (cargo.layer == 6)
        {
            blueScore += scoreMultiplier;
            Instantiate(BlueCargo, shot);
        }
        if (cargo.layer == 7)
        {
            redScore += scoreMultiplier;
            Instantiate(RedCargo, shot);
        }
    }

    public int BScore()
    {
        return blueScore;
    }

    public int RScore()
    {
        return redScore;
    }
}
