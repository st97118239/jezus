using UnityEngine;
using UnityEngine.AI;

public class EnemyVelocityTracker : MonoBehaviour
{
    public Vector3 Velocity { get; private set; }

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        Velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    public Vector3 PredictFuturePosition(float timeAhead)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        float speed = GetComponent<Enemy>().speed;
        float distanceAhead = speed * timeAhead;

        Vector3 predictedPosition = transform.position;

        if (agent.path == null || agent.path.corners.Length < 2)
            return predictedPosition;

        for (int i = 0; i < agent.path.corners.Length - 1; i++)
        {
            Vector3 from = agent.path.corners[i];
            Vector3 to = agent.path.corners[i + 1];
            float segmentLength = Vector3.Distance(from, to);

            if (distanceAhead > segmentLength)
                distanceAhead -= segmentLength;
            else
            {
                predictedPosition = Vector3.Lerp(from, to, distanceAhead / segmentLength);
                return predictedPosition;
            }
        }

        return agent.path.corners[^1];
    }
}
