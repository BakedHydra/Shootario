using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float Damage;
    [SerializeField] private float Speed;
    public void Initialize(Vector3 vector)
    {
        GetComponent<Rigidbody>().AddForce(vector.normalized * Speed, ForceMode.Impulse);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().GetDamage(Damage);
        }
        Destroy(gameObject);
    }
}
