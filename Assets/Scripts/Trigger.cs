using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent OnZoneEntered;
    private bool isUsed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isUsed)
        {
            OnZoneEntered.Invoke();
            isUsed = true;
        }
    }
}
