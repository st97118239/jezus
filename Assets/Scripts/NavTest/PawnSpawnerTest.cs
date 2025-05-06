using UnityEngine;
using UnityEngine.AI;

public class PawnSpawnerTest : MonoBehaviour
{
    public GameObject pawn;
    public Vector3 spawnPos;
    public bool canSpawn = true;
    public float spawnTimer = 1;
    public float spawnTimerBase = 1;

    private void Update()
    {
        if (canSpawn)
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
}
