using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject Brobot;
    public GameObject Rrobot;
    public Collider[] colliders;
    public LayerMask mask;
    public float rad;
    public int AllianceSize;
    int BluePop;
    int RedPop;
    public Vector3 PlayerLocation;
    public bool isBlue;
    public bool playerLocUsed;

    void Start() 
    {
        BluePop = AllianceSize;
        RedPop = AllianceSize;
        if (isBlue && playerLocUsed) 
        {
            GameObject robo = Instantiate(Brobot, PlayerLocation, Quaternion.identity) as GameObject;
            BluePop -= 1;
        }
        else if (playerLocUsed)
        {
            GameObject robo = Instantiate(Rrobot, PlayerLocation, Quaternion.identity) as GameObject;
            RedPop -= 1;
        }

        for (int i = 0; i < BluePop; i++) 
        {
            SpawnRobo(Brobot);
        }
        for (int i = 0; i < RedPop; i++)
        {
            SpawnRobo(Rrobot);
        }
    }

    void SpawnRobo(GameObject robot) 
    {
        Vector3 spawnPos = new Vector3(0f, 0f, 0f);
        bool canSpawnHere = false;
        int failsafe = 0;

        while (!canSpawnHere) 
        {
            float spawnX = Random.Range(-5.5f, 5.5f);
            float spawnZ = Random.Range(-2.5f, 2.5f);
            spawnPos = new Vector3(spawnX, 0.23f, spawnZ);
            canSpawnHere = PreventOverlap(spawnPos);

            if (canSpawnHere)
            {
                break;
            }

            failsafe++;

            if (failsafe > 50)
            {
                Debug.Log("madly unlucky you are");
                break;
            }
        }

        GameObject robo = Instantiate(robot, spawnPos, Quaternion.identity) as GameObject;
    }

    bool PreventOverlap(Vector3 spawn) 
    {
        colliders = Physics.OverlapSphere(transform.position, rad, mask);
        
        for (int i = 0; i < colliders.Length; i++) 
        {
            Vector3 center = colliders[i].gameObject.transform.position;
            float width = colliders[i].gameObject.transform.localScale.x;
            float height = colliders[i].gameObject.transform.localScale.y;

            float leftSide = center.x - 1f - width/2;
            float rightSide = center.x + 1f + width/2;
            float downSide = center.y - 1f - height/2;
            float upSide = center.y + 1f + height/2;

            if ((spawn.x > leftSide) && (spawn.x < rightSide)) 
            {
                if ((spawn.y > downSide) && (spawn.y < upSide))
                {
                    return false;
                }
            }
        }

        return true;
    }

}
