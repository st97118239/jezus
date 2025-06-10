using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerType type;
    public BarracksTower barracksTower;
    public MeshRenderer mesh;
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
    public bool hasNoShooter;

    private Main main;
    private Range rangeObject;
    private BarracksRange barracksRangeObject;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        reloadSpeedBase = reloadSpeed;
        rangeBase = range;
        projectileSpeedBase = projectileSpeed;
        damageBase = damage;
        main = FindObjectOfType<Main>();
        rangeObject = FindObjectOfType<Range>();
        if (!hasNoShooter)
            shooter = transform.Find("Shooter").GetComponent<Shooter>();
        else if (type == TowerType.Barracks)
        {
            barracksRangeObject = FindObjectOfType<BarracksRange>();
            barracksTower = GetComponent<BarracksTower>();
        }
    }

    public void Select()
    {
        rangeObject.transform.position = transform.position;
        rangeObject.transform.localScale = new Vector3(range * 2, 0.1f, range * 2);
        rangeObject.GetComponent<MeshRenderer>().enabled = true;

        if (type == TowerType.Barracks)
        {
            BaseUnit unit = barracksTower.units[barracksTower.upgradeCount];
            float barracksRange = barracksTower.unitRange + unit.extraDistanceToFindEnemiesIn;
            barracksRangeObject.transform.position = barracksTower.destinationBall.transform.position;
            barracksRangeObject.transform.localScale = new Vector3(barracksRange * 2, 0.1f, barracksRange * 2);
            barracksRangeObject.GetComponent<MeshRenderer>().enabled = true;
        }

        main.ChangeLayerOfAllDescendants(transform, 9);

        if (type == TowerType.Barracks)
            main.bus.NewTowerSelected(GetComponent<BarracksTower>());
        else if (type == TowerType.SuicideBombers)
            main.sus.NewTowerSelected(GetComponent<BomberTower>());
        else
            main.tus.NewTowerSelected(this);
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
        if (type == TowerType.Barracks)
        {
            main.bus.TowerDeselected();
            barracksRangeObject.transform.localScale = new Vector3(0f, 0f, 0f);
            barracksRangeObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else if (type == TowerType.SuicideBombers)
            main.sus.TowerDeselected();
        else
            main.tus.TowerDeselected();

        main.ChangeLayerOfAllDescendants(transform, 0);
    }

    public void TurnShooterOn()
    {
        if (!shooter)
            shooter = transform.Find("Shooter").GetComponent<Shooter>();

        shooter.GetComponent<Shooter>().enabled = true;
        shooter.canShoot = true;
    }
}
