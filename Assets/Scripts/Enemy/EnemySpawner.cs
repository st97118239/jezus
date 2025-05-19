using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> activePawns = new();
    public List<GameObject> toSpawn;
    public List<Transform> waypoints;
    public GameObject pawn;
    public int currentWave = 0;
    public float spawnTimerBase = 1;
    public bool canSpawn = true;
    public bool nextWave;

    [SerializeField] private Vector3 spawnPos;

    private Main main;
    private float spawnTimer = 1;

    private void Start()
    {
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();
    }

    private void Update()
    {
        if (canSpawn)
        {
            if (spawnTimer < 0)
            {
                spawnTimer = spawnTimerBase;

                GameObject newObject = Instantiate(pawn, spawnPos, Quaternion.identity);
                activePawns.Add(newObject);

                newObject.SetActive(true);
            }
            else
                spawnTimer -= Time.deltaTime;
        }
    }

    public void EnemyKilled(int coinsToReceive, GameObject enemyToRemove)
    {
        main.ChangeCoinAmount(coinsToReceive);
        activePawns.Remove(enemyToRemove);
    }
}
