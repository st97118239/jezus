using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> activeEnemies = new();
    public List<int> enemiesUsedInWavesAmount;
    public List<GameObject> toSpawn;
    public Transform waypointParent;
    public List<Transform> waypoints = new List<Transform>();
    public bool canSpawn = true;

    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private Vector3 spawnRotOffset;
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int startingWaypoint = 0;
    [SerializeField] private int groupSpawnCount = 1;
    [SerializeField] private float spawnTimerBase = 1;
    [SerializeField] private float nextWaveTimerBase = 10;

    private Queue<EnemyToSpawn> enemiesToSpawn = new();
    private Main main;
    private Vector3 spawnPos;
    private Vector3 spawnRot;
    private float nextWaveTimer;
    private float spawnTimer;
    private bool nextWave;

    void Awake()
    {
        if (waypointParent != null)
        {
            waypoints.Clear();
            foreach (Transform child in waypointParent)
            {
                waypoints.Add(child);
            }

            waypoints = waypointParent
            .GetComponentsInChildren<Transform>()
            .Where(t => t != waypointParent)
            .OrderBy(t =>
            {
                Match match = Regex.Match(t.name, @"\d+");
                return match.Success ? int.Parse(match.Value) : 0;
            })
            .ToList();
        }
    }

    private void Start()
    {
        spawnPos = waypoints[startingWaypoint].transform.position;
        spawnRot = waypoints[startingWaypoint].transform.rotation.eulerAngles + spawnRotOffset;

        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();

        spawnTimer = spawnTimerBase;
        nextWaveTimer = nextWaveTimerBase;

        foreach (GameObject enemy in enemyList)
        {
            enemiesUsedInWavesAmount.Add(0);
        }

        nextWave = true;
    }

    private void Update()
    {
        if (nextWave)
        {
            if (nextWaveTimer <= 0)
                GenerateWave(++currentWave);
            else
                nextWaveTimer -= Time.deltaTime;
        }


        if (canSpawn)
        {
            if (spawnTimer < 0)
            {
                if (activeEnemies.Count == 0 && enemiesToSpawn.Count == 0)
                {
                    nextWave = true;
                }
                else if (enemiesToSpawn.Count > 0)
                {
                    spawnTimer = spawnTimerBase;

                    for (int i = 0; i < groupSpawnCount; i++)
                    {
                        EnemyToSpawn enemyToSpawn = enemiesToSpawn.Dequeue();
                        GameObject gameObjectToSpawn = enemyList[(int)enemyToSpawn.type];

                        GameObject newObject = Instantiate(gameObjectToSpawn, spawnPos, Quaternion.Euler(spawnRot));

                        Enemy newEnemy = newObject.GetComponent<Enemy>();
                        newEnemy.health = (int)enemyToSpawn.health;
                        newEnemy.damage = enemyToSpawn.damage;
                        newEnemy.nav.currentWPIndex = startingWaypoint;

                        activeEnemies.Add(newObject);
                        newObject.SetActive(true);
                    }

                    main.ip.RedrawWaveText(currentWave, activeEnemies.Count, enemiesToSpawn.Count);
                }


                spawnTimer = spawnTimerBase;
            }
            else
                spawnTimer -= Time.deltaTime;
        }
    }

    public void EnemyKilled(int coinsToReceive, GameObject enemyToRemove)
    {
        main.ChangeCoinAmount(coinsToReceive);
        activeEnemies.Remove(enemyToRemove);
        main.ip.RedrawWaveText(currentWave, activeEnemies.Count, enemiesToSpawn.Count);
    }

    public void GenerateWave(int wave)
    {
        enemiesToSpawn.Clear();

        int index = 0;

        foreach (GameObject enemy in enemyList)
        {
            Enemy enemyType = enemy.GetComponent<Enemy>();

            if (enemyType.startingWave > wave || (wave - enemyType.startingWave) % enemyType.waveModulo != 0)
                continue;

            int enemyUsedInWavesCount = enemiesUsedInWavesAmount[index];

            int amountOfEnemyType = (int)Math.Floor(enemyType.enemyAmount + enemyType.amountFactor * enemyUsedInWavesCount);
            int health = (int)Math.Floor(enemyType.health + enemyType.healthFactor * enemyUsedInWavesCount);
            int damage = (int)Math.Floor(enemyType.damage + enemyType.damageFactor * enemyUsedInWavesCount);

            enemiesUsedInWavesAmount[index]++;
            Debug.Log(amountOfEnemyType + " " + enemyType + " spawned with " + health + " HP and " + damage + " damage.");

            for (int i = 0; i < amountOfEnemyType; i++)
            {
                enemiesToSpawn.Enqueue(new EnemyToSpawn(enemyType.enemyType, health, damage));
            }

            index++;
        }

        nextWaveTimer = nextWaveTimerBase;
        nextWave = false;
        main.ip.RedrawWaveText(wave, activeEnemies.Count, enemiesToSpawn.Count);
    }
}
