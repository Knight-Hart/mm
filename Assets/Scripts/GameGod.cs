using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class GameGod : MonoBehaviour
{

    public GameObject MegaMan;
    public GameObject BeeEnemy;
    public GameObject WalkingBall;
    public int numOfEnemies = 20;
    public int RegionDensity = 1;

    private GameObject currentMegaMan;
    private GameObject[] enemies;
    private MegaManController megaManContoller;

    private bool init = false;
    void Start()
    {
        InvokeRepeating("SpawnAllEnemeies", 5, 20);
    }

    void Update()
    {
        if (!init)
        {
            CallGodToRespawn();
            init = true;
        }

    }

    public void CallGodToRespawn()
    {
        currentMegaMan = GameObject.FindGameObjectWithTag("Player");
        megaManContoller = currentMegaMan.GetComponent<MegaManController>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Camera.main.transform.position = new Vector3(5.5f, -0.03373f, -10f);
        Destroy(currentMegaMan);

        for (int i = 0; i < enemies.Length - 1; i++)
        {
            Destroy(enemies[i]);
        }

        Instantiate(MegaMan);

        SpawnAllEnemeies();
    }
    void SpawnAllEnemeies()
    {
        //int enemyNumReg1 = Random.Range(1, (numOfEnemies / RegionDensity));
        //int enemyNumReg2 = Random.Range(1, (numOfEnemies / RegionDensity));
        //int enemyNumReg3 = Random.Range(1, (numOfEnemies / RegionDensity));
        //int enemyNumReg4 = Random.Range(1, (numOfEnemies / RegionDensity));

        //RegionInstantiate(enemyNumReg1, 10f, 15f, 0.8f, 1.8f);
        //RegionInstantiate(enemyNumReg2, 16f, 26f, 1.4f, 1.8f);
        //RegionInstantiate(enemyNumReg3, 27f, 46f, 1.2f, 1.8f);
        //RegionInstantiate(enemyNumReg4, 10f, 15f, 0.8f, 1.8f);


        void RegionInstantiate(int numEnemies, float xMin, float xMax, float yMin, float yMax)
        {
            for (int i = 0; i < numEnemies; i++)
            {
                int enemyType = Random.Range(0, 4);
                float randX = Random.Range(xMin, xMax);
                float randY = Random.Range(yMin, yMax);
                if (enemyType != 0)
                    Instantiate(BeeEnemy, new Vector3(randX, randY, 0), Quaternion.identity);
                else
                    Instantiate(WalkingBall, new Vector3(randX, randY, 0), Quaternion.identity);
            }
        }
    }
}
