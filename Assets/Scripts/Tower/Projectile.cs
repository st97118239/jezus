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
    private bool canMove;

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
        canMove = true;
        damage = givenDamage;
        despawnTimer = despawnTimerAmount;
        shooter = attacker;
    }

    void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude > 0.01f) // Check to avoid NaNs from zero velocity
        {
            Quaternion rotation = Quaternion.LookRotation(rb.velocity);
            transform.rotation = rotation;
        }
    }

    //private void Update()
    //{
    //    if (canMove)
    //    {
    //        float step = speed * Time.deltaTime;
    //        transform.position = Vector3.MoveTowards(transform.position, target, step);

    //        if (transform.position == target)
    //        {
    //            if (despawnTimer > 0)
    //                despawnTimer -= Time.deltaTime;
    //            else
    //            {
    //                Destroy(gameObject);
    //            }
    //        }
    //    }
    //}

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
