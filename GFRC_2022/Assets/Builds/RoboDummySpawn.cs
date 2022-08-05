using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboDummySpawn : MonoBehaviour
{
    public GameObject Dummy;
    public Collider[] colliders;
    public LayerMask mask;
    public float rad;
    public int AllianceSize;

    void Start() 
    {

        for (int i = 0; i < AllianceSize; i++) 
        {
            SpawnRobo(Dummy);
        }
    }

    void SpawnRobo(GameObject robot) 
    {
        Vector3 spawnPos = new Vector3(0f, 0f, 0f);
        bool canSpawnHere = false;
        int failsafe = 0;

        while (!canSpawnHere) 
        {
            float spawnX = Random.Range(-5f, 5f);
            float spawnZ = Random.Range(-3.5f, 3.5f);
            spawnPos = new Vector3(spawnX, 0.5f, spawnZ);
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
            float downSide = center.z - 1f - height/2;
            float upSide = center.z + 1f + height/2;

            if ((spawn.x > leftSide) && (spawn.x < rightSide)) 
            {
                if ((spawn.z > downSide) && (spawn.z < upSide))
                {
                    return false;
                }
            }
        }

        return true;
    }

}
