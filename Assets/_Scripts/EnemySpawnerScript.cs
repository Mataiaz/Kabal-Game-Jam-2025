using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject path;
    public DamWallScript damWall;
    private List<Transform> pathSteps = new List<Transform>();

    bool isSpawning = false;

    void Start() {
        foreach (Transform t in path.transform) {
            pathSteps.Add(t);
        }
    }


    public void SpawnEnemies(GameObject enemy, int amount, float delayBetweenIteration)
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(HandleSpawnEnemies(enemy, amount, delayBetweenIteration));
        }
    }


    private IEnumerator HandleSpawnEnemies(GameObject enemy, int iteration, float delay) {

        int i = iteration;

        if (i > 50) {
            i = 50;
        }

        while (i > 0) {
            GameObject spawnedEnemy = Instantiate(enemy, pathSteps[0].position, Quaternion.identity);
            EnemyScript enemyScript = spawnedEnemy.GetComponent<EnemyScript>();
            enemyScript.targets = pathSteps;
            enemyScript.wallTarget = damWall;
            yield return new WaitForSeconds(delay);
            i--;
        }
        
    }

}
