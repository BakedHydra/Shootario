using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float Damage;
    private float Speed;
    public void Initialize(Vector3 vector, float damage, float speed)
    {
        Damage = damage;
        Speed = speed;
        GetComponent<Rigidbody>().AddForce(transform.forward * Speed, ForceMode.Impulse);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().GetDamage(Damage);
        }
        else if (other.gameObject.CompareTag("Projectile"))
        {
            return;
        }
            Destroy(gameObject);
    }
}
