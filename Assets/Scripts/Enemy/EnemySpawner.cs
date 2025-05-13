using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> activePawns = new();
    public List<Transform> waypoints;
    public GameObject pawn;
    public float spawnTimerBase = 1;
    public bool canSpawn = true;

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
        main.coinsAmount = main.coinsAmount + coinsToReceive;
        activePawns.Remove(enemyToRemove);
    }
}
