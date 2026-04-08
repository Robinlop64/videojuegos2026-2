using UnityEngine;
public class CameraController : MonoBehaviour
{
    public Transform target;
    public Transform pivot;
    public float sensitivity = 3f;
    public float minY = -20f;
    public float maxY = 60f;
    float rotX;
    float rotY;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        rotX += mouseX;
        rotY -= mouseY;
        rotY = Mathf.Clamp(rotY, minY, maxY);
        transform.rotation = Quaternion.Euler(0, rotX, 0);
        pivot.localRotation = Quaternion.Euler(rotY, 0, 0);
        transform.position = target.position;
    }
}