using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool cursorLock;
    public Vector3 moveDir;
    public float speed;
    public float zoomSpeed;
    public Vector3 velocity;
    public float zoomTime;

    private void Start()
    {
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    void Update()
    {
        moveDir.z = Input.GetAxis("Horizontal");
        moveDir.x = -Input.GetAxis("Vertical");
        transform.Translate(moveDir * Time.deltaTime * speed);

        moveDir.z = 0;
        moveDir.x = 0;
        float dir = Input.GetAxis("Mouse ScrollWheel");
        if (dir < 0f)
            moveDir.y = dir;
        else if (dir > 0f)
            moveDir.y = dir;

        Vector3 desiredPosition = transform.position - (Vector3.up * moveDir.y * zoomSpeed);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, zoomTime);
        moveDir.y = 0;
    }
}
