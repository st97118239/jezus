using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Shooter shooter;
    public Material defaultMaterial;
    public Material transparentMaterial;
    public int price;
    public int upgradePrice;
    public float health;
    public float reloadSpeed;
    public float range;
    public float projectileSpeed;
    public float projectileDespawnTime = 0.1f;
    public float damage;
    public bool recentlyBuilt = true;

    [SerializeField] private float timeToBuild = 1;

    private Main main;
    private GameObject rangeObject;
    private float buildTimer;

    private void Start()
    {
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();
        rangeObject = transform.Find("Range").gameObject;
        shooter = transform.Find("Shooter").GetComponent<Shooter>();
        buildTimer = timeToBuild;
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
            rangeObject.transform.localScale = new Vector3(range * 2, 0.1f, range * 2);
            rangeObject.GetComponent<MeshRenderer>().enabled = true;
        }
        main.tus.NewTowerSelected(this);
    }

    public void Deselect()
    {
        rangeObject.transform.localScale = new Vector3(0f, 0f, 0f);
        rangeObject.GetComponent<MeshRenderer>().enabled = false;
        main.tus.TowerDeselected();
    }
}
