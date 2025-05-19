using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> activeEnemies = new();
    public Queue<EnemyToSpawn> enemiesToSpawn = new();
    public List<GameObject> toSpawn;
    public List<GameObject> enemyList;
    public List<int> enemiesUsedInWavesAmount;
    public List<Transform> waypoints;
    public GameObject pawn;
    public int currentWave = 0;
    public float spawnTimerBase = 1;
    public float nextWaveTimer;
    public float nextWaveTimerBase = 10;
    public bool canSpawn = true;
    public bool nextWave;

    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private Vector3 spawnRotation;

    private Main main;
    private float spawnTimer = 1;

    private void Start()
    {
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();

        spawnTimer = spawnTimerBase;
        nextWaveTimer = nextWaveTimerBase;

        foreach (GameObject enemy in enemyList)
        {
            enemiesUsedInWavesAmount.Add(0);
        }

        GenerateWave(++currentWave);
    }

    private void Update()
    {
        if (nextWave)
        {
            if (nextWaveTimer <= 0)
            {
                GenerateWave(++currentWave);
                nextWaveTimer = nextWaveTimerBase;
                nextWave = false;
            }
            else
            {
                nextWaveTimer -= Time.deltaTime;
            }
        }


        if (canSpawn)
        {
            if (spawnTimer < 0)
            {
                if (activeEnemies.Count == 0 && enemiesToSpawn.Count == 0)
                {
                    nextWave = true;
                    //waveText.text = "Wave " + (currentWave + 1).ToString();
                }
                else if (enemiesToSpawn.Count > 0)
                {
                    spawnTimer = spawnTimerBase;

                    EnemyToSpawn enemyToSpawn= enemiesToSpawn.Dequeue();
                    GameObject gameObjectToSpawn = enemyList[(int)enemyToSpawn.type];

                    GameObject newObject = Instantiate(gameObjectToSpawn, spawnPos,
                        Quaternion.Euler(spawnRotation));

                    // Stel health, damage en speed in!
                    Enemy newEnemy = newObject.GetComponent<Enemy>();
                    newEnemy.health = (int)enemyToSpawn.health;
                    newEnemy.damage = (int)enemyToSpawn.damage;

                    activeEnemies.Add(newObject);
                    newObject.SetActive(true);
                }


                spawnTimer = spawnTimerBase;

                //GameObject newObject = Instantiate(pawn, spawnPos, Quaternion.identity);
                //activeEnemies.Add(newObject);

                //newObject.SetActive(true);
            }
            else
                spawnTimer -= Time.deltaTime;
        }
    }

    public void EnemyKilled(int coinsToReceive, GameObject enemyToRemove)
    {
        main.ChangeCoinAmount(coinsToReceive);
        activeEnemies.Remove(enemyToRemove);
    }

    public void GenerateWave(int wave)
    {
        enemiesToSpawn.Clear();

        int index = 0;

        foreach (GameObject enemy in enemyList)
        {
            Enemy enemyType = enemy.GetComponent<Enemy>();

            if (enemyType.startingWave > wave)
                continue;

            if (wave % enemyType.waveModulo != 0)
                continue;

            int enemyUsedInWavesCount = enemiesUsedInWavesAmount[index];

            int amountOfEnemyType = (int)Mathf.Floor(enemyType.enemyAmount + enemyType.amountFactor * enemyUsedInWavesCount);
            enemiesUsedInWavesAmount[index]++;
            Debug.Log("amount of " + enemyType + ": " + amountOfEnemyType);

            for (int i = 0; i < amountOfEnemyType; i++)
            {
                enemiesToSpawn.Enqueue(new EnemyToSpawn(enemyType.enemyType, enemyType.health, enemyType.damage));
            }

            index++;
        }
    }
}
