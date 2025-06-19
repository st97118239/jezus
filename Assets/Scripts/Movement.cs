using UnityEngine;

public class Movement : MonoBehaviour
{
    public Main main;
    public bool canMove = true;

    [SerializeField] private Vector3 minBoundsZoomedIn;
    [SerializeField] private Vector3 maxBoundsZoomedIn;
    [SerializeField] private Vector3 minBoundsZoomedOut;
    [SerializeField] private Vector3 maxBoundsZoomedOut;
    [SerializeField] private bool cursorLock;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private float speed;
    [SerializeField] private float zoomSpeedBase;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float zoomTime;
    [SerializeField] private Vector3 lowestZoomPos;
    [SerializeField] private Vector3 highestZoomPos;
    [SerializeField] private float shiftSpeedFactor = 2;

    private float zoomSpeed;

    private void Start()
    {
        main = FindAnyObjectByType<Main>();

        if (cursorLock)
            Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        zoomSpeed = zoomSpeedBase / main.timeScale;

        if (canMove)
        {
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

            moveDir.z = Input.GetAxis("Vertical");
            moveDir.x = Input.GetAxis("Horizontal");
            transform.Translate(moveDir * (Time.deltaTime / main.timeScale) * speed);

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
                transform.position = new Vector3(transform.position.x, lowestZoomPos.y, transform.position.z);
            else if (transform.position.y > highestZoomPos.y)
                transform.position = new Vector3(transform.position.x, highestZoomPos.y, transform.position.z);

            Vector3 clampedPosition = transform.position;

            float zoomFactor = Mathf.InverseLerp(lowestZoomPos.y, highestZoomPos.y, transform.position.y);
            Vector3 minBounds = Vector3.Lerp(minBoundsZoomedIn, minBoundsZoomedOut, zoomFactor);
            Vector3 maxBounds = Vector3.Lerp(maxBoundsZoomedIn, maxBoundsZoomedOut, zoomFactor);

            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, minBounds.z, maxBounds.z);

            transform.position = clampedPosition;
        }
    }

    public void DisableMovement()
    {
        canMove = false;
    }
}
