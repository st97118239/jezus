using UnityEngine;

public class Tower : MonoBehaviour
{
    public float health;
    public int price;
    public float reloadSpeed;
    public float range;
    public float projectileSpeed;
    public float damage;

    [SerializeField] private int upgradePrice;

    private Shooter shooter;

    private void Start()
    {
        shooter = transform.Find("Shooter").GetComponent<Shooter>();
        print(shooter);
    }
}
