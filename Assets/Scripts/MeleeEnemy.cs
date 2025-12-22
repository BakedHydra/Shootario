using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy: Enemy
{
    [Header("Melee Enemy Settings")]
    [SerializeField] private float MeleeDamage = 0;

    private void Start()
    {
        Health = MaxHealth;
        navMeshAgent = GetComponent<NavMeshAgent>();
        material = GetComponent<Renderer>().material;
        startColor = material.GetColor("_BaseColor");
        canMove = true;
        target = GameObject.FindWithTag("Player").transform;
    }
    private void Update()
    {
        IEnumerator GiveDamageCoroutine()
        {
            canDamage = false;
            target.gameObject.GetComponent<PlayerController>().GetDamage(MeleeDamage);
            yield return new WaitForSeconds(1.5f);
            canDamage = true;
        }
        if (!canMove) { return; }
        if (navMeshAgent == null) { return; }
        if (target == null) { return; }
        navMeshAgent.SetDestination(target.position);
        float distance = (target.position - transform.position).magnitude;
        if (distance <= 3 && canDamage == true)
        {
            StartCoroutine(GiveDamageCoroutine());
        }
    }
}
