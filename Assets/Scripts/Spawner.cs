using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public Transform[] spawnLocations;
    public Transform[] topDownSpawnLocations;
    public GameObject[] normalPrefab;
    public GameObject[] normalClone;
    public GameObject[] topDownPrefab;
    public GameObject[] topDownClone;
    


    void Start()
    {
        spawnMovingPlatform();

    }


    void spawnMovingPlatform()
    {
        for(int i = 0; i < normalClone.Length; i++)
        {
            normalClone[i] = Instantiate(normalPrefab[i], spawnLocations[i].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            normalClone[i].gameObject.name = "MovingPlatform" + i;
        }

    }

    void spawnTopDownMovingPlatform() {
        for (int i = 0; i < topDownClone.Length; i++)
        {
            topDownClone[i] = Instantiate(topDownPrefab[i], topDownSpawnLocations[i].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            topDownClone[i].gameObject.name = "TopDownMovingPlatform" + i;
        }
    }


	
}
