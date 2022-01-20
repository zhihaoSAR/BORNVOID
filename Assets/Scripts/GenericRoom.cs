using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericRoom : RoomManager
{
    [SerializeField]
    Transform[] SpawnPoints;
    [SerializeField]
    RoomConfig config;
    int round;
    int enemiesSpawned;
    int enemiesDead;
    float[] probability;
    int enemyNum;
    [SerializeField]
    bool firstRoom = false;
    [SerializeField]
    GameObject exitWall, enterWall;

    public override void enemyDead()
    {
        if (++enemiesDead == enemyNum)
        {
            roomCompleted();
        }
    }

    public override void playerEnter()
    {
        StartCoroutine(Spawn());
        if (enterWall)
        {
            enterWall.SetActive(true);
        }
    }

    public override void playerExit()
    {
        return;
    }


    // Start is called before the first frame update
    void Start()
    {
        probability = new float[config.Enemies.Length];
        for (int i = 0; i < config.Enemies.Length; i++)
        {
            probability[i] = config.InitProbability[i];
        }
        enemyNum = config.InitEnemyNum;
        enemiesDead = 0;
        enemiesSpawned = 0;
        if (firstRoom)
        {
            playerEnter();
        }



    }

    IEnumerator Spawn()
    {
        while (enemiesSpawned < enemyNum)
        {
            int spawnNum = UnityEngine.Random.Range(0, Math.Min((enemyNum - enemiesSpawned), config.MaxEnemieNumAtTime - enemiesSpawned + enemiesDead) + 1);
            for (int i = 0; i < spawnNum; i++)
            {
                Vector3 spawnPos = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)].position;
                float p = UnityEngine.Random.Range(0f, 1f);
                int enemyInd;
                for (enemyInd = 0; enemyInd < config.Enemies.Length; enemyInd++)
                {
                    if (p <= probability[enemyInd])
                    {
                        break;
                    }
                    p -= probability[enemyInd];
                }
                Debug.Log("new enemy");
                Enemy e = Instantiate(config.Enemies[enemyInd], spawnPos, Quaternion.identity);
                e.setDeadCallback(enemyDead);
                enemiesSpawned++;
            }
            yield return new WaitForSeconds(config.SpawnTime);
        }
    }

    public override void roomCompleted()
    {
        Debug.Log("pasada");
        if (exitWall)
        {
            exitWall.SetActive(false);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnter();
        }
    }

}
