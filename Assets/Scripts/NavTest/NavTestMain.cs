using UnityEngine;
using TMPro;
using UnityEngine.AI;
using Unity.VisualScripting;

public class NavTestMain : MonoBehaviour
{
    public int health = 100;
    public bool isDead = false;

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private PawnSpawnerTest pst;
    [SerializeField] private GameObject gameoverText;

    private void Start()
    {
        pst = FindObjectOfType(typeof(PawnSpawnerTest)).GetComponent<PawnSpawnerTest>();
    }

    public void ReceiveDmg(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            health = 0;
            GameOver();
        }

        healthText.text = "" + health.ToString();
    }

    private void GameOver()
    {
        print("ded");
        isDead = true;
        gameoverText.SetActive(true);
        pst.canSpawn = false;
        foreach (GameObject pawn in pst.activePawns)
        {
            pawn.GetComponent<NavigationTest>().canMove = false;
            pawn.GetComponent<NavMeshAgent>().enabled = false;
        }
    }
}
