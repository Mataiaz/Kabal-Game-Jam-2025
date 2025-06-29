using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManagerScript : MonoBehaviour
{
    int round = 0;
    public EnemySpawnerScript[] spawners;
    bool validatingRoundConditions = false;
    bool roundStarted = false;
    public GameObject enemy;
    int enemyCount = 0;
    public Slider slider;
    public TMPro.TMP_Text waterStatus;
    public TMPro.TMP_Text damStatus;
    private int damTotHealth = 1000;
    private int damWaterLvl = 1000;
    public int leakCount = 0;

    public GameObject gameOverScreen;

    bool isGameOver = false;

    public DamWallScript[] damWallScripts;

    bool isValidating = false;

  void Update()
  {
        if (Input.GetKeyDown(KeyCode.R) && isGameOver)
        {
            SceneManager.LoadScene(0);
        }
  }

  void FixedUpdate()
    {
        if (!roundStarted)
        {
            roundStarted = true;
            round++;
            StartRound(round);
        }

        if (roundStarted && !isValidating)
        {
            isValidating = true;
            StartCoroutine(ValidateRound(5f));
        }

        if (damWaterLvl < 1000)
        {
            UpdateWaterLevel(1);
        }
    }

    void Start()
    {
        InitGame();
        isGameOver = false;
        Time.timeScale = 1;
        AudioListener.volume = 1f;
        gameOverScreen.SetActive(false);
    }

    void InitGame()
    {
        int i = 0;
        foreach (EnemySpawnerScript ess in spawners)
        {
            ess.damWall = damWallScripts[i];
            i++;
        }
        waterStatus.text = "Great";
        damStatus.text = "No leaks detected";

        waterStatus.color = Color.green;
        damStatus.color = Color.green;

    }

    void StartRound(int round)
    {
        int enemyToSpawn;
        foreach (EnemySpawnerScript ess in spawners)
        {
            enemyToSpawn = Random.Range(5 * round, 10 * round);
            ess.SpawnEnemies(enemy, enemyToSpawn, Random.Range(1, 3));
            enemyCount = enemyToSpawn + enemyToSpawn;
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        AudioListener.volume = 0f;
        gameOverScreen.SetActive(true);

    }

    public void UpdateDamHealth()
    {
        damTotHealth = 0;
        foreach (DamWallScript dam in damWallScripts)
        {
            damTotHealth = damTotHealth + dam.health;
        }


        if (damTotHealth > 950)
        {
            waterStatus.text = "Great";
        }
        else if (damTotHealth < 950)
        {
            waterStatus.text = "Good";
        }

        else if (damTotHealth < 800)
        {
            waterStatus.text = "Okey";
            waterStatus.color = Color.orange;
        }
        else if (damTotHealth < 500)
        {
            waterStatus.text = "Bad";
            waterStatus.color = Color.red;

        }
        else if (damTotHealth < 300)
        {
            waterStatus.text = "Critical";
            waterStatus.color = Color.red;

        }
    }

    public void UpdateDamLeakStatus(int leakCountChange)
    {
        if (leakCountChange <= 0)
        {
            if (leakCount - leakCountChange <= 0)
            {
                leakCount = 0;
            }
            else
            {
                leakCount = leakCount - leakCountChange;
            }
        }
        else
        {
            leakCount = leakCount + leakCountChange;
        }

        if (leakCount > 5)
        {
            damStatus.text = leakCount.ToString() + " leaks detected";
            damStatus.color = Color.red;
        }
        else if (leakCount > 0)
        {
            damStatus.text = leakCount.ToString() + " leaks detected";
            damStatus.color = Color.orange;
        }
        else
        {
            damStatus.text = "No leaks detected";
            damStatus.color = Color.green;
        }

    }

    public void UpdateWaterLevel(int value)
    {
        damWaterLvl = damWaterLvl - value;
        slider.value = damWaterLvl / 1000;
    }

    private IEnumerator ValidateRound(float delay)
    {
        yield return new WaitForSeconds(delay);
        enemyCount = 0;
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyCount++;
        }
        if (enemyCount <= 0)
        {
            isValidating = false;
            roundStarted = false;
        }
    }

}
