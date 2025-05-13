using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool cursorLock;

    private void Start()
    {
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
