using UnityEngine;
public class CameraCollision : MonoBehaviour
{
    public Transform pivot;
    public float maxDistance = 4f;
    public float minDistance = 0.5f;
    public float smooth = 10f;
    float currentDistance;
    void Start()
    {
        currentDistance = maxDistance;
    }
    void LateUpdate()
    {
        Vector3 dir = (transform.position - pivot.position).normalized;
        if (Physics.Raycast(pivot.position, dir, out RaycastHit hit,
        maxDistance))
        {

            currentDistance = Mathf.Clamp(hit.distance, minDistance,

            maxDistance);
        }
        else
        {
            currentDistance = maxDistance;
        }
        transform.position = Vector3.Lerp(
        transform.position,
        pivot.position + dir * currentDistance,
        Time.deltaTime * smooth
        );
    }
}