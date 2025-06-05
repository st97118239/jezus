using UnityEngine;

public class SuicideBomber : MonoBehaviour
{
    private Tower tower;
    private bool isReloading;
    private float reloadSpeed;
    private float reloadTimer;

    private void Start()
    {
        tower = GetComponent<Tower>();
        reloadSpeed = tower.reloadSpeed;
    }

    private void Update()
    {
        if (isReloading)
        {
            if (reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;
            }
            else
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        Debug.Log("shoot");
        isReloading = true;
        reloadTimer = reloadSpeed;
    }
}
