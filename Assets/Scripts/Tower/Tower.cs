using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerTypes type;
    public Shooter shooter;
    public int price;
    public float reloadSpeed;
    public float reloadSpeedBase;
    public float range;
    public float rangeBase;
    public float projectileSpeed;
    public float projectileSpeedBase;
    public float projectileDespawnTime = 0.1f;
    public float damage;
    public float damageBase;
    public bool recentlyBuilt = true;

    [SerializeField] private float timeToBuild = 1;

    private Main main;
    private Range rangeObject;
    private BarracksRange barracksRangeObject;
    private BarracksTower barracksTower;
    private float buildTimer;

    private void Start()
    {
        reloadSpeedBase = reloadSpeed;
        rangeBase = range;
        projectileSpeedBase = projectileSpeed;
        damageBase = damage;
        buildTimer = timeToBuild;
        main = FindObjectOfType<Main>();
        rangeObject = FindObjectOfType<Range>();
        if (type != TowerTypes.Barracks)
            shooter = transform.Find("Shooter").GetComponent<Shooter>();
        else
        {
            barracksRangeObject = FindObjectOfType<BarracksRange>();
            barracksTower = GetComponent<BarracksTower>();
        }
    }

    private void Update()
    {
        if (recentlyBuilt)
        {
            if (buildTimer < 0)
            {
                recentlyBuilt = false;
            }
            else
                buildTimer -= Time.deltaTime;
        }
    }

    public void Select()
    {
        if (!recentlyBuilt)
        {
            rangeObject.transform.position = transform.position;
            rangeObject.transform.localScale = new Vector3(range * 2, 0.1f, range * 2);
            rangeObject.GetComponent<MeshRenderer>().enabled = true;

            if (type == TowerTypes.Barracks)
            {
                Unit unit = barracksTower.units[barracksTower.upgradeCount];
                float barracksRange = barracksTower.unitRange + unit.extraDistanceToFindEnemiesIn;

                barracksRangeObject.transform.position = barracksTower.destinationBall.transform.position;
                barracksRangeObject.transform.localScale = new Vector3(barracksRange * 2, 0.1f, barracksRange * 2);
                barracksRangeObject.GetComponent<MeshRenderer>().enabled = true;
            }
            else if (type == TowerTypes.Barracks)
                Debug.Log(recentlyBuilt);
        }

        if (type != TowerTypes.Barracks)
            main.tus.NewTowerSelected(this);
        else
            main.bus.NewTowerSelected(GetComponent<BarracksTower>());
    }

    public void RedrawRange()
    {
        if (rangeObject != null)
        {
            rangeObject.transform.position = transform.position;
            rangeObject.transform.localScale = new Vector3(range * 2, 0.1f, range * 2);
            rangeObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
            rangeObject = FindObjectOfType<Range>();
    }

    public void WhenPlaced()
    {
        rangeObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void Deselect()
    {
        rangeObject.transform.localScale = new Vector3(0f, 0f, 0f);
        rangeObject.GetComponent<MeshRenderer>().enabled = false;
        if (type != TowerTypes.Barracks)
            main.tus.TowerDeselected();
        else
        {
            main.bus.TowerDeselected();
            barracksRangeObject.transform.localScale = new Vector3(0f, 0f, 0f);
            barracksRangeObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void TurnShooterOn()
    {
        if (!shooter)
            shooter = transform.Find("Shooter").GetComponent<Shooter>();

        shooter.GetComponent<Shooter>().enabled = true;
        shooter.canShoot = true;
    }
}
