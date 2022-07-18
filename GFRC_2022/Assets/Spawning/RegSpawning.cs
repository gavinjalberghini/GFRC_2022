using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegSpawning : MonoBehaviour
{
    public GameObject[] Robots;

    public int robotType;
    public int chosenSpawn;
    public bool isRandom;
    public int allianceSize;

    public Transform SpawnPoint1;
    public Transform SpawnPoint2;
    public Transform SpawnPoint3;
    public Transform SpawnPoint4;

    // Start is called before the first frame update
    void Start()
    {
        int[] spawns = { 1, 2, 3, 4 };
        int i = 0, j = 0, temp = 0;
        if (isRandom)
        {
            for (i = 0; i < 4; i++)
            {
                j = Random.Range(1, 4);

                temp = spawns[i];
                spawns[i] = spawns[j];
                spawns[j] = temp;
            }

            for (i = 0; i < allianceSize; i++)
            {
                Spawn(Robots[Random.Range(1, Robots.Length)], spawns[i]);
            }
        }
        else 
        {
            Spawn(Robots[robotType], chosenSpawn);
            for (i = 0; i < 4; i++)
            {
                j = Random.Range(1, 4);

                temp = spawns[i];
                spawns[i] = spawns[j];
                spawns[j] = temp;
            }

            for (i = 0; i < allianceSize; i++)
            {
                if (spawns[i] != chosenSpawn) 
                { 
                    Spawn(Robots[Random.Range(1, Robots.Length)], spawns[i]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn(GameObject robo, int spawnSpot)
    {
        Transform shot = SpawnPoint1;
        switch (spawnSpot)
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
        Instantiate(robo, shot);
    }
}
