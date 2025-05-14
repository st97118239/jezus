using UnityEngine;
using TMPro;
using UnityEngine.AI;
using Unity.VisualScripting;
using TMPro.EditorUtilities;

public class Main : MonoBehaviour
{
    public int coinsAmount;
    public int defaultCoinsAmount = 50;
    public int health;
    public int defaultHealth = 100;
    public bool isDead = false;

    private TMP_Text healthText;
    private TMP_Text coinText;
    private GameObject gameoverPanel;
    private EnemySpawner es;

    private void Start()
    {
        es = FindObjectOfType(typeof(EnemySpawner)).GetComponent<EnemySpawner>();
        gameoverPanel = transform.Find("GameOverPanel").gameObject;
        healthText = transform.Find("HPText").GetComponent<TMP_Text>();
        coinText = transform.Find("CoinText").GetComponent<TMP_Text>();
        coinsAmount = defaultCoinsAmount;
        RedrawCoinText();
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

    public void ChangeCoinAmount(int coinsToReceive)
    {
        coinsAmount += coinsToReceive;

        if (coinsAmount < 0)
        {
            coinsAmount = 0;
        }

        RedrawCoinText();
    }

    public void RedrawHealthBar()
    {
        healthText.text = "" + health.ToString();
    }

    public void RedrawCoinText()
    {
        coinText.text = "" + coinsAmount.ToString();
    }

    private void GameOver()
    {
        print("ded");
        isDead = true;
        gameoverPanel.SetActive(true);
        es.canSpawn = false;
        foreach (GameObject pawn in es.activePawns)
        {
            pawn.GetComponent<EnemyNavigation>().canMove = false;
            pawn.GetComponent<NavMeshAgent>().enabled = false;
        }
    }
}
