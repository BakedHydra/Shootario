using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Basic Enemy Settings")]
    public float MaxHealth = 0;
    public float Health { get; protected set; }
    [SerializeField] protected GameObject Parent;
    public static event Action OnDeath;

    protected NavMeshAgent navMeshAgent;
    protected bool canMove;
    protected Material material;
    protected Transform target = null;
    protected bool canDamage = true;
    protected Color startColor;

    public virtual void GetDamage(float damage, RaycastHit hit)
    {
        IEnumerator GetDamageCoroutine()
        {
            material.SetColor("_BaseColor", Color.red);
            navMeshAgent.enabled = false;
            canMove = false;
            GetComponent<Rigidbody>().isKinematic = false;
            Vector3 hitVector = (hit.point - target.position).normalized;
            GetComponent<Rigidbody>().AddForce(hitVector * 10, ForceMode.Impulse);
            yield return new WaitForSeconds(0.25f);
            GetComponent<Rigidbody>().isKinematic = true;
            navMeshAgent.enabled = true;
            canMove = true;
            material.SetColor("_BaseColor", startColor);
        }

        Health -= damage;
        StartCoroutine(GetDamageCoroutine());
        if (Health <= 0)
        {
            GetKilled();
        }
    }
    public virtual void GetKilled()
    {
        OnDeath?.Invoke();
        Destroy(Parent);
    }
}
