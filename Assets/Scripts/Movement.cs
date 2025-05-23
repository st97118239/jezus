using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private bool cursorLock;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private float speed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float zoomTime;
    [SerializeField] private Vector3 lowestZoomPos;
    [SerializeField] private Vector3 highestZoomPos;
    [SerializeField] private float shiftSpeedFactor = 2;

    private void Start()
    {
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed *= shiftSpeedFactor;
            zoomSpeed *= shiftSpeedFactor;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed /= shiftSpeedFactor;
            zoomSpeed /= shiftSpeedFactor;
        }


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

        if (transform.position.y < lowestZoomPos.y)
            transform.position = new Vector3 (transform.position.x, lowestZoomPos.y, transform.position.z);
        else if (transform.position.y > highestZoomPos.y)
            transform.position = new Vector3(transform.position.x, highestZoomPos.y, transform.position.z);
    }
}
