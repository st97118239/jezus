using Unity.VisualScripting;
using UnityEngine;

public class Predict : MonoBehaviour
{
    public float timeAhead = 1f; // How far ahead we predict the enemy's position
    public Transform target; // Enemy's transform
    public float projectileSpeed = 10f; // Speed of the projectile
    public GameObject projectilePrefab;
    public bool hasShot = false;

    void Update()
    {
        if (target != null && !hasShot)
        {
            // Predict the enemy's future position
            Vector3 predictedPosition = PredictEnemyPosition(target);

            // Calculate direction to the predicted position
            Vector3 direction = (predictedPosition - transform.position).normalized;
            
            // Fire the projectile
            FireProjectile(direction, predictedPosition);
        }
    }

    // Method to predict the enemy's position
    Vector3 PredictEnemyPosition(Transform enemy)
    {
        // Get enemy's current position and velocity (assuming the enemy has a Rigidbody)
        Vector3 enemyPosition = enemy.position;
        Vector3 enemyVelocity = enemy.GetComponent<Rigidbody>().velocity; // Adjust this if your movement is custom

        // Predict the future position
        Vector3 predictedPosition = enemyPosition + enemyVelocity * timeAhead;

        print(predictedPosition);
        return predictedPosition;
    }

    // Method to fire a projectile (simplified example)
    void FireProjectile(Vector3 direction, Vector3 predictedPosition)
    {
        hasShot = true;
        // Here you would instantiate and shoot a projectile towards the predicted direction
        // This is just a placeholder for whatever projectile logic you have
        Transform spawnLocation = transform.Find("Shooter").transform.Find("ProjSpawn").GetComponent<Transform>();
        GameObject projectile = Instantiate(projectilePrefab, spawnLocation.position, Quaternion.identity);
        //Rigidbody rb = projectile.GetComponent<Rigidbody>();
        //rb.velocity = direction * projectileSpeed;
        projectile.GetComponent<Projectile>().Move(predictedPosition);
        

        //GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.SetActive(true);
    }
}
