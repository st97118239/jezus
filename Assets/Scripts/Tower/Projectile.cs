using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 target;
    private float speed;
    private float damage;
    private float despawnTimer;
    private bool canMove;

    public void Move(Vector3 predictedPosition, float givenSpeed, float givenDamage, float despawnTimerAmount)
    {
        target = predictedPosition;
        transform.LookAt(target);
        speed = givenSpeed;
        canMove = true;
        damage = givenDamage;
        despawnTimer = despawnTimerAmount;
    }

    private void Update()
    {
        if (canMove)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            if (transform.position == target)
            {
                if (despawnTimer > 0)
                    despawnTimer -= Time.deltaTime;
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Enemy>().GotHit(damage);
        }

        Destroy(gameObject);
    }
}
