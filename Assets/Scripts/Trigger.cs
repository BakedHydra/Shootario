using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent OnZoneEntered;
    protected bool isUsed = false;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isUsed)
        {
            OnZoneEntered.Invoke();
            isUsed = true;
        }
    }
}
