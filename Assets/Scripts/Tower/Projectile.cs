using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool canMove;
    private Vector3 target;
    private float speed;
    private float damage;

    public void Move(Vector3 predictedPosition, float givenSpeed, float givenDamage)
    {
        target = predictedPosition;
        transform.LookAt(target);
        speed = givenSpeed;
        canMove = true;
        damage = givenDamage;
    }

    private void Update()
    {
        if (canMove)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
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
