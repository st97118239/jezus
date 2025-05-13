using UnityEngine;

public class ProjectileTest : MonoBehaviour
{
    public Vector3 target;
    public Vector3 reset;
    public float speed;

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }
}
