using UnityEngine;
using UnityEngine.AI;

public class Main : MonoBehaviour
{
    public EnemySpawner es;
    public ClickDetection clickDetec;
    public InfoPanel ip;
    public EnemyPanel ep;
    public UnitPanel up;
    public MenuButtons mb;
    public TowerUpgradeSystem tus;
    public BarracksUpgradeSystem bus;
    public SuicideUpgradeSystem sus;
    public int coinsAmount;
    public int defaultCoinsAmount = 50;
    public int health;
    public int defaultHealth = 100;
    public bool isDead = false;

    private TowerPlacement tp;
    private GameObject gameoverPanel;

    private void Start()
    {
        clickDetec = FindObjectOfType<ClickDetection>();
        es = FindObjectOfType<EnemySpawner>();
        tus = FindObjectOfType<TowerUpgradeSystem>();
        bus = FindObjectOfType<BarracksUpgradeSystem>();
        sus = FindObjectOfType<SuicideUpgradeSystem>();
        tp = FindObjectOfType<TowerPlacement>();
        ep = FindObjectOfType<EnemyPanel>();
        up = FindObjectOfType<UnitPanel>();
        ep.gameObject.SetActive(false);
        up.gameObject.SetActive(false);
        GameObject upgradePanel = transform.Find("UpgradePanel").gameObject;
        GameObject barracksPanel = transform.Find("BarracksPanel").gameObject;
        GameObject suicidePanel = transform.Find("SuicidePanel").gameObject;
        upgradePanel.SetActive(false);
        tus.FindUpgradePanel(upgradePanel);
        bus.FindBarracksPanel(barracksPanel);
        sus.FindSuicidePanel(suicidePanel);
        ip = transform.Find("InfoPanel").GetComponent<InfoPanel>();
        gameoverPanel = transform.Find("GameOverPanel").gameObject;
        coinsAmount = defaultCoinsAmount;
        ip.RedrawCoinText(coinsAmount);
        health = defaultHealth;
        ip.RedrawHealthBar(health);
        ip.RedrawWaveText(0, 0, 0);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mb.gameObject.activeSelf == true)
            {
                mb.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                mb.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }
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

    public void ChangeLayerOfAllDescendants(Transform tf, int layer)
    {
        if (!tf.CompareTag("NoLayerToggle"))
            tf.gameObject.layer = layer;

        foreach (Transform child in tf)
        {
            ChangeLayerOfAllDescendants(child, layer);
        }
    }
}
