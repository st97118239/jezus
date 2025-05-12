using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool canMove;
    private Vector3 moveTo;

    public void Move(Vector3 predictedPosition)
    {
        moveTo = predictedPosition;
        transform.LookAt(moveTo);
        canMove = true;
    }

    private void Update()
    {
        if (canMove)
        {
            Vector3.MoveTowards(transform.position, moveTo, Time.deltaTime * 10);
        }
        
    }
}
