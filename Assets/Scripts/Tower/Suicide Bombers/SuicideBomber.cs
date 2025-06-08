using UnityEngine;

public class SuicideBomber : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private BomberTower tower;
    private Vector3 target;
    private float speed;
    private float damage;
    private float despawnTimer;
    private bool reachedMax;

    public void SetStats(float givenDamage, Vector3 landingPos, BomberTower givenTower)
    {
        damage = givenDamage;
        target = landingPos;
        tower = givenTower;
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
        if (rb.velocity.sqrMagnitude > 0.01f && reachedMax)
        {
            Quaternion rotation = Quaternion.LookRotation(rb.velocity);
            transform.rotation = rotation;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("SuicideBomberMaxHeight"))
        {
            PrepareShoot();
            reachedMax = true;
            tower.isFlyingUp = false;
        }
    }

    private void PrepareShoot()
    {
        float h = Mathf.Abs(target.y - transform.position.y);

        // Calculate time of flight (t = sqrt(2h / |gravity|))
        float t = Mathf.Sqrt(2 * h / Mathf.Abs(Physics.gravity.magnitude));

        // Horizontal distance between object and target
        float d = Mathf.Abs(target.x - transform.position.x);

        // Required horizontal velocity to hit target
        float vx = d / t;

        // Apply force (impulse) in horizontal direction
        rb.useGravity = true;
        rb.AddForce(rb.mass * vx * transform.forward, ForceMode.Impulse);

        //Vector3 velocity = CalculateLaunchVelocityWithAngle(transform.position, target, 0);
        //if (velocity != Vector3.zero)
        //{
        //    SetVelocity(velocity);
        //}
    }

    public Vector3 CalculateLaunchVelocity()
    {
        // Displacement vector
        Vector3 displacement = target - transform.position;

        float dx = displacement.x;
        float dy = displacement.y;
        float dz = displacement.z;

        float velocityFactor = 2 * dy / Physics.gravity.magnitude;
        int negative = velocityFactor < 0 ? -1 : 1;
        float t = Mathf.Sqrt(velocityFactor * negative) * negative;

        // Calculate velocity in x and z directions
        float vx = dx / t;
        float vz = dz / t;

        return new Vector3(vx, 0, vz);
    }

    public static Vector3 CalculateLaunchVelocityWithAngle(Vector3 origin, Vector3 target, float launchAngleInDegrees, float gravity = 9.81f)
    {
        // Displacement
        Vector3 displacement = target - origin;
        float dx = displacement.x;
        float dz = displacement.z;

        float angleRad = Mathf.Deg2Rad * launchAngleInDegrees;

        // Calculate horizontal velocity (vx)
        float vx = dx / Mathf.Cos(angleRad);

        // Time of flight is determined by horizontal distance and speed
        float t = dx / vx;

        // Calculate vertical velocity (vz) using time of flight and displacement
        float vz = (dz + 0.5f * gravity * t * t) / t;

        return new Vector3(vx, 0, vz);
    }
}
