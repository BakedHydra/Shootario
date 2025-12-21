using UnityEngine;

public class Destroyable : MonoBehaviour
{
    [SerializeField] private float Health;
    public void GetDamage(float damage, RaycastHit hit)
    {
        void GetKilled()
        {
            Destroy(gameObject);
        }
        Health -= damage;
        if (Health <= 0)
        {
            GetKilled();
        }
    }
}
