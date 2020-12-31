using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public float spawnTime;
    public float maxSpawnTime;
    private float tempSpawnTime;

    private List<int> indexToBeSelected = new List<int>();

    System.Random random = new System.Random();

    public List<GameObject> obstacles;
    public float obstacleSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        int times = 0;
        for (int i = 0; i < obstacles.Count; i++)
        {
            if (i == 0)
                times = 15;
            else if (i == 1)
                times = 7;
            else if (i == 2)
                times = 3;
            else if (i == 3)
                times = 3;

            for (int j = 0; j < times; j++)
            {
                indexToBeSelected.Add(i);
            }

        }

        //tempSpawnTime = spawnTime;
        tempSpawnTime = (float)random.NextDouble() * (maxSpawnTime - spawnTime) + spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (tempSpawnTime < 0)
        {
            int randInt = random.Next(0, indexToBeSelected.Count);
            GameObject obs = Instantiate(obstacles[indexToBeSelected[randInt]], obstacles[indexToBeSelected[randInt]].transform.position, Quaternion.identity) as GameObject;
            obs.GetComponent<ObstacleScript>().speed = obstacleSpeed;

            //tempSpawnTime = spawnTime;
            tempSpawnTime = (float)random.NextDouble() * (maxSpawnTime - spawnTime) + spawnTime;
        }
        else
        {
            tempSpawnTime -= Time.deltaTime;
        }
    }
}
