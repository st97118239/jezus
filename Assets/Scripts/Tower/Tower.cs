using UnityEngine;

public class Tower : MonoBehaviour
{
    public int price;
    public int upgradePrice;
    public float health;
    public float reloadSpeed;
    public float range;
    public float projectileSpeed;
    public float damage;
    public bool recentlyBuilt = true;

    [SerializeField] private float timeToBuild;

    private Shooter shooter;
    private GameObject rangeObject;
    private float buildTimer;

    private void Start()
    {
        shooter = transform.Find("Shooter").GetComponent<Shooter>();
        rangeObject = transform.Find("Range").gameObject;
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
        rangeObject.transform.localScale = new Vector3(range * 2, 0.1f, range * 2);
        rangeObject.SetActive(true);
    }

    public void Deselect()
    {
        rangeObject.SetActive(false);
    }
}
