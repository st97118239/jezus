using UnityEngine;

public class Tower : MonoBehaviour
{
    public int health;
    public int price;

    [SerializeField] private int damage;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int upgradePrice;
    [SerializeField] private float range;

    private Shooter shooter;

    private void Start()
    {
        shooter = transform.Find("Shooter").GetComponent<Shooter>();
        print(shooter);
    }
}
