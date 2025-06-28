using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManagerScript : MonoBehaviour
{
    int round = 0;
    public EnemySpawnerScript[] spawners;
    bool validatingRoundConditions = false;
    bool roundStarted = false;
    public GameObject enemy;
    int enemyCount = 0;

    void FixedUpdate()
    {
        if (!roundStarted)
        {
            roundStarted = true;
            round++;
            StartRound(round);
        }
    }

    void StartRound(int round)
    {
        int enemyToSpawn;
        foreach (EnemySpawnerScript ess in spawners)
        {
            enemyToSpawn = Random.Range(1 * round, 5 * round);
            ess.SpawnEnemies(enemy, enemyToSpawn, Random.Range(1, 3));
            enemyCount = enemyToSpawn + enemyToSpawn;
        }
        //StartCoroutine(ValidateRound());
    }

    //private IEnumerator ValidateRound()
    //{
    //    yield return new WaitForSeconds(5);
//
    //    
    //    while (actualEnemyCount)
    //        yield return new WaitForSeconds(delay);
    //}
    
}
