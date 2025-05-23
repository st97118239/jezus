using UnityEngine;
using UnityEngine.AI;
using Unity.VisualScripting;

public class Main : MonoBehaviour
{
    public InfoPanel ip;
    public TowerUpgradeSystem tus;
    public int coinsAmount;
    public int defaultCoinsAmount = 50;
    public int health;
    public int defaultHealth = 100;
    public bool isDead = false;

    private EnemySpawner es;
    private TowerPlacement tp;
    private GameObject gameoverPanel;

    private void Start()
    {
        es = FindObjectOfType(typeof(EnemySpawner)).GetComponent<EnemySpawner>();
        tus = FindObjectOfType(typeof(TowerUpgradeSystem)).GetComponent<TowerUpgradeSystem>();
        tp = FindObjectOfType(typeof(TowerPlacement)).GetComponent<TowerPlacement>();
        GameObject upgradePanel = transform.Find("UpgradePanel").gameObject;
        upgradePanel.SetActive(false);
        tus.FindUpgradePanel(upgradePanel);
        ip = transform.Find("InfoPanel").GetComponent<InfoPanel>();
        gameoverPanel = transform.Find("GameOverPanel").gameObject;
        coinsAmount = defaultCoinsAmount;
        ip.RedrawCoinText(coinsAmount);
        health = defaultHealth;
        ip.RedrawHealthBar(health);
        ip.RedrawWaveText(0, 0, 0);
    }

    public void ReceiveDmg(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            health = 0;
            GameOver();
        }

        ip.RedrawHealthBar(health);
    }

    public void ChangeCoinAmount(int coinsToReceive)
    {
        coinsAmount += coinsToReceive;

        if (coinsAmount < 0)
        {
            coinsAmount = 0;
        }

        ip.RedrawCoinText(coinsAmount);
    }

    

    private void GameOver()
    {
        print("ded");
        isDead = true;
        gameoverPanel.SetActive(true);
        es.canSpawn = false;
        foreach (GameObject pawn in es.activeEnemies)
        {
            pawn.GetComponent<EnemyNavigation>().canMove = false;
            pawn.GetComponent<NavMeshAgent>().enabled = false;
        }

        foreach (Tower tower in tp.towersPlaced)
        {
            tower.shooter.canShoot = false;
        }
    }
}
