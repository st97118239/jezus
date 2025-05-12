using UnityEngine;

public class Tower : MonoBehaviour
{
    public int health;
    public int price;
    public float reloadSpeed;
    public float range;


    [SerializeField] private int damage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int upgradePrice;

    private Shooter shooter;

    private void Start()
    {
        shooter = transform.Find("Shooter").GetComponent<Shooter>();
        print(shooter);
    }

   
}
