using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 velocity;
    private Rigidbody rb;

    private Shooter shooter;
    private Enemy currentTarget;
    private Vector3 target;
    private float speed;
    private float damage;
    private float despawnTimer;

    public void SetStats(Shooter givenShooter, float givenDamage, Enemy givenTarget)
    {
        shooter = givenShooter;
        damage = givenDamage;
        currentTarget = givenTarget;
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = velocity;
    }

    public void Move(Vector3 predictedPosition, float givenSpeed, float givenDamage, float despawnTimerAmount, Shooter attacker)
    {
        target = predictedPosition;
        transform.LookAt(target);
        speed = givenSpeed;
        damage = givenDamage;
        despawnTimer = despawnTimerAmount;
        shooter = attacker;
    }

    void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(rb.velocity);
            transform.rotation = rotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Enemy>().GotHit(damage);
        }
        else if (!collision.gameObject.GetComponent<Tower>())
            currentTarget.tempHealth += damage;

        Destroy(gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //        other.GetComponent<Enemy>().GotHit(damage);
    //    else
    //        currentTarget.tempHealth += damage;

    //    Destroy(gameObject);
    //}
}
