using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX, sensY;
    float xRotation, yRotation;
    public Transform playerBody;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX * 100;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY * 100;
        
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
