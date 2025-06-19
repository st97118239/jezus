using UnityEngine;

public class Main : MonoBehaviour
{
    public OutlineSelection os;
    public EnemySpawner es;
    public ClickDetection clickDetec;
    public InfoPanel ip;
    public EnemyPanel ep;
    public UnitPanel up;
    public MenuButtons mb;
    public TowerUpgradeSystem tus;
    public BarracksUpgradeSystem bus;
    public SuicideUpgradeSystem sus;
    public AudioManager am;
    public int coinsAmount;
    public int defaultCoinsAmount = 50;
    public int health;
    public int defaultHealth = 100;
    public float musicVolume = 1;
    public float soundVolume = 1;
    public bool isFinished = false;

    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;

    private TowerPlacement tp;
    private Movement movement;

    private void Start()
    {
        movement = FindObjectOfType<Movement>();
        os = FindObjectOfType<OutlineSelection>();
        clickDetec = FindObjectOfType<ClickDetection>();
        es = FindObjectOfType<EnemySpawner>();
        tus = FindObjectOfType<TowerUpgradeSystem>();
        bus = FindObjectOfType<BarracksUpgradeSystem>();
        sus = FindObjectOfType<SuicideUpgradeSystem>();
        am = FindObjectOfType<AudioManager>();
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
        coinsAmount = defaultCoinsAmount;
        ip.RedrawCoinText(coinsAmount);
        health = defaultHealth;
        ip.RedrawHealthBar(health);
        ip.RedrawWaveText(0, 0, 0);
        LoadSettings();
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (!isFinished && Input.GetKeyDown(KeyCode.Escape))
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
            Debug.Log("Castle got destroyed, 0 hp left.");
            Lose();
        }

        ip.RedrawHealthBar(health);
    }

    public void ChangeCoinAmount(int coinsToReceive)
    {
        coinsAmount += coinsToReceive;

        if (coinsAmount < 0)
            coinsAmount = 0;

        ip.RedrawCoinText(coinsAmount);
    }

    public void Win()
    {
        DisableGame();

        winCanvas.SetActive(true);
        if (am.winSound)
            am.winSound.Play();
    }

    public void Lose()
    {
        DisableGame();

        loseCanvas.SetActive(true);
        if (am.loseSound)
            am.loseSound.Play();
    }

    public void DisableGame()
    {
        isFinished = true;
        es.DisableSpawner();
        movement.DisableMovement();

        foreach (GameObject enemy in es.activeEnemies)
            enemy.GetComponent<Enemy>().DisableEnemy();

        foreach (Tower tower in tp.towersPlaced)
            tower.DisableTower();

        Time.timeScale = 0;
    }

    public void LoadSettings()
    {
        Settings settings = FindObjectOfType<Settings>();
        if (settings)
            settings.LoadIntoGame();
        
        am.LoadVolumeSettings(musicVolume, soundVolume);
    }
}
