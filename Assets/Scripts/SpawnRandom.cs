using UnityEngine;
using System.Collections;

public class SpawnRandom : MonoBehaviour
{

    public GameObject[] objects;
    public float spawnTime = 3f;
    private Vector3 spawnPosition;

    void Start()
    {

        InvokeRepeating("Spawn", spawnTime, spawnTime);

    }

    void Spawn()
    {
        spawnPosition.x = Random.Range(0, 46);
        spawnPosition.y = 1.6f;
        spawnPosition.z = Random.Range(0, 0);

        Instantiate(objects[Random.Range(0, objects.Length - 1)], spawnPosition, Quaternion.identity);
    }

}
