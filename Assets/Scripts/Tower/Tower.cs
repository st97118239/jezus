using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerTypes type;
    public Shooter shooter;
    public Material defaultMaterial;
    public Material transparentMaterial;
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
    private float buildTimer;

    private void Start()
    {
        reloadSpeedBase = reloadSpeed;
        rangeBase = range;
        projectileSpeedBase = projectileSpeed;
        damageBase = damage;
        buildTimer = timeToBuild;
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();
        rangeObject = FindObjectOfType(typeof(Range)).GetComponent<Range>();
        if (type != TowerTypes.Barracks)
            shooter = transform.Find("Shooter").GetComponent<Shooter>();
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
        }
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
            rangeObject = FindObjectOfType(typeof(Range)).GetComponent<Range>();
    }

    public void WhenPlaced()
    {
        rangeObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void Deselect()
    {
        rangeObject.transform.localScale = new Vector3(0f, 0f, 0f);
        rangeObject.GetComponent<MeshRenderer>().enabled = false;
        main.tus.TowerDeselected();
    }
}
