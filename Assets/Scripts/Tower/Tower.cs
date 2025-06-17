using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerType type;
    public BarracksTower barracksTower;
    public BomberTower bomberTower;
    public MeshRenderer mesh;
    public Shooter shooter;
    public Range rangeObject;
    public BarracksRange barracksRangeObject;
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

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        reloadSpeedBase = reloadSpeed;
        rangeBase = range;
        projectileSpeedBase = projectileSpeed;
        damageBase = damage;
        main = FindObjectOfType<Main>();
        if (!hasNoShooter)
            shooter = transform.Find("Shooter").GetComponent<Shooter>();
        else if (type == TowerType.Barracks)
            barracksTower = GetComponent<BarracksTower>();
        else if (type == TowerType.SuicideBombers)
            bomberTower = GetComponent<BomberTower>();
    }

    public void Select()
    {
        rangeObject.transform.position = transform.position;
        rangeObject.transform.localScale = new Vector3(range * 2, 0.1f, range * 2);
        rangeObject.GetComponent<MeshRenderer>().enabled = true;

        if (type == TowerType.Barracks)
            barracksTower.Selected(true);
        else
            main.os.ChangeLayerOfAllDescendants(transform, 9);

        if (type == TowerType.Barracks)
            main.bus.NewTowerSelected(barracksTower);
        else if (type == TowerType.SuicideBombers)
            bomberTower.Selected(true);
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
            barracksTower.Deselected(true);
        else if (type == TowerType.SuicideBombers) 
            bomberTower.Deselected();
        else
            main.tus.TowerDeselected();

        main.os.ChangeLayerOfAllDescendants(transform, 10);
    }

    public void TurnShooterOn()
    {
        if (!shooter)
            shooter = transform.Find("Shooter").GetComponent<Shooter>();

        shooter.GetComponent<Shooter>().enabled = true;
        shooter.canShoot = true;
    }

    public void DisableTower()
    {
        if (shooter)
            shooter.canShoot = false;

        if (type == TowerType.Barracks)
            barracksTower.DisableTower();
        else if (type == TowerType.SuicideBombers)
            bomberTower.DisableTower();
    }
}
