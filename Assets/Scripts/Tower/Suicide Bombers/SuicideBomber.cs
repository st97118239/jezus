using System.Linq;
using UnityEditor;
using UnityEngine;

public class SuicideBomber : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MeshRenderer rend;
    [SerializeField] private MeshRenderer rend2;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private float explosionRadius;

    private BomberTower tower;
    private Vector3 target;
    private float speed;
    private float damage;
    private bool reachedMax;
    private bool hitGround;

    public void SetStats(float givenDamage, float givenSpeed, Vector3 landingPos, BomberTower givenTower)
    {
        damage = givenDamage;
        target = landingPos;
        tower = givenTower;
        speed = givenSpeed;
    }

    private void Update()
    {
        if (!reachedMax)
        {
            Vector3 targetDirection = target - transform.position;
            targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 200);
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    void FixedUpdate()
    {
        if (!hitGround && rb.velocity.sqrMagnitude > 0.01f && reachedMax)
        {
            Quaternion rotation = Quaternion.LookRotation(rb.velocity);
            transform.rotation = rotation;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("SuicideBomberMaxHeight"))
        {
            Vector3 directionToTarget = (target - transform.position).normalized;
            rb.velocity = directionToTarget * speed;
            reachedMax = true;
            tower.isFlyingUp = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        tower.spawnedBombers.Remove(this);
        hitGround = true;
        particles.Play();
        boxCollider.enabled = false;
        rend.enabled = false;
        rend2.enabled = false;
        Destroy(rb);
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in hitColliders)
        {
            if (tower.main.es.activeEnemies.Contains(collider.gameObject))
            {
                collider.GetComponent<Enemy>().GotHit(damage);
            }
        }
    }
}
