using UnityEngine;
using UnityEngine.AI;

public class PawnSpawnerTest : MonoBehaviour
{
    public GameObject pawn;
    public Vector3 spawnPos;
    public float spawnTimer;
    public float spawnTimerBase;

    private void Start()
    {
        spawnTimerBase = 1;
        spawnTimer = 1;
    }

    private void Update()
    {
        if (spawnTimer < 0)
        {
            spawnTimer = spawnTimerBase;

            GameObject newObject = Instantiate(pawn, spawnPos, Quaternion.identity);

            newObject.SetActive(true);
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }
}
