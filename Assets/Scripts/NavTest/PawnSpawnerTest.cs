using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnSpawnerTest : MonoBehaviour
{
    public List<GameObject> activePawns = new();
    public List<Transform> waypoints;
    public GameObject pawn;
    public float spawnTimerBase = 1;
    public bool canSpawn = true;

    [SerializeField] private Vector3 spawnPos;

    private float spawnTimer = 1;

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
}
