using UnityEngine;
using TMPro;
using UnityEngine.AI;
using Unity.VisualScripting;

public class Main : MonoBehaviour
{
    public int coinsAmount;
    public int defaultCoinsAmount = 50;
    public int health;
    public int defaultHealth = 100;
    public bool isDead = false;

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private EnemySpawner es;
    [SerializeField] private GameObject gameoverText;

    private void Start()
    {
        es = FindObjectOfType(typeof(EnemySpawner)).GetComponent<EnemySpawner>();
        coinsAmount = defaultCoinsAmount;
        health = defaultHealth;
        RedrawHealthBar();
    }

    public void ReceiveDmg(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            health = 0;
            GameOver();
        }

        RedrawHealthBar();
    }

    public void RedrawHealthBar()
    {
        healthText.text = "" + health.ToString();
    }

    private void GameOver()
    {
        print("ded");
        isDead = true;
        gameoverText.SetActive(true);
        es.canSpawn = false;
        foreach (GameObject pawn in es.activePawns)
        {
            pawn.GetComponent<EnemyNavigation>().canMove = false;
            pawn.GetComponent<NavMeshAgent>().enabled = false;
        }
    }
}
