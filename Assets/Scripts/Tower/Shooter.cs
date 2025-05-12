using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private PawnSpawnerTest pst;
    private List<GameObject> shootableEnemies = new();

    private void Start()
    {
        pst = FindObjectOfType(typeof(PawnSpawnerTest)).GetComponent<PawnSpawnerTest>();
    }

    private void Update()
    {
        shootableEnemies.Clear();
        //shootableEnemies = 
    }
}
