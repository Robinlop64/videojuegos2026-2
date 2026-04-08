using UnityEngine;
using System;
public class DoorTrigger : MonoBehaviour
{
    public event Action OnDoorOpen;
    public event Action OnDoorClose;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnDoorOpen?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnDoorClose?.Invoke();
        }

    }
}