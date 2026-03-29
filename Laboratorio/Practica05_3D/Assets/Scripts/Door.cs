using UnityEngine;
public class Door : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 2, 0);
    private bool isOpen = false;
    public DoorTrigger doorTrigger; // Asignar en Inspector
    private void OnEnable()
    {
        if (doorTrigger != null)
        {
            doorTrigger.OnDoorOpen += OpenDoor;
            doorTrigger.OnDoorClose += CloseDoor;
        }
    }
    private void OnDisable()
    {
        if (doorTrigger != null)
        {
            doorTrigger.OnDoorOpen -= OpenDoor;
            doorTrigger.OnDoorClose -= CloseDoor;
        }
    }
    public void OpenDoor()
    {
        if (!isOpen)
        {
            transform.position += openOffset;
            isOpen = true;
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            transform.position -= openOffset;
            isOpen = false;
        }
    }
}