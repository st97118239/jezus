using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool cursorLock;
    public Vector3 moveDir;
    public float speed;

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
    }
}
