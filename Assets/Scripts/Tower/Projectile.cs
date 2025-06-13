using UnityEditor.Rendering;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float hitRange;

    private Shooter shooter;
    private Enemy currentTarget;
    private Vector3 target;
    private bool usesArcedProj;
    private float damage;
    private float timeToReach;
    private float timer;
    private float speed;

    public void SetStats(Shooter givenShooter, float givenDamage, Enemy givenTarget, float time, float givenSpeed)
    {
        shooter = givenShooter;
        damage = givenDamage;
        currentTarget = givenTarget;
        timeToReach = time;
        timer = timeToReach;
        speed = givenSpeed;
    }

    public void SetVelocity(Vector3 velocity, bool arcedProjectiles, float forwardAmount)
    {
        rb.velocity = velocity + (transform.forward * forwardAmount);
        rb.useGravity = arcedProjectiles;
        if (arcedProjectiles)
            usesArcedProj = true;
    }

    void Update()
    {
        if (currentTarget)
            target = currentTarget.transform.position;

        if (!usesArcedProj && timer > 0 && timer > (timeToReach * 0.25))
        {
            timer -= Time.deltaTime;
        }
        else if (usesArcedProj && timer > 0 && timer > (timeToReach * 0.5))
        {
            timer -= Time.deltaTime;
        }
        else if (timer < 0)
        {
            if (currentTarget)
            {
                if (usesArcedProj)
                    transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, speed / 2 * Time.deltaTime);
                else
                    transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, speed * Time.deltaTime);

                transform.LookAt(currentTarget.transform.position);
            }
            else
            {
                if (usesArcedProj)
                    transform.position = Vector3.MoveTowards(transform.position, target, speed / 2 * Time.deltaTime);
                else
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

                transform.LookAt(target);
            }


        }
        else
        {
            timer = -1;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;

        }
    }

    void FixedUpdate()
    {
        if (timer > 0 && rb.velocity.sqrMagnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(rb.velocity);
            transform.rotation = rotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, hitRange);

        foreach (Collider collider in hitColliders)
        {
            if (currentTarget && collider.gameObject == currentTarget.gameObject)
            {
                currentTarget.GotHit(damage);
            }
            else if (currentTarget && !collision.gameObject.GetComponent<Tower>())
                currentTarget.tempHealth += damage;
        }

        Destroy(gameObject);

        //    if (collision.gameObject.GetComponent<Enemy>())
        //    {
        //        collision.gameObject.GetComponent<Enemy>().GotHit(damage);
        //    }
        //    else if (!collision.gameObject.GetComponent<Tower>())
        //        currentTarget.tempHealth += damage;

        //    Destroy(gameObject);
    }
}
