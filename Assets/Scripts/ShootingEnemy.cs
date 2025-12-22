using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy: Enemy
{
    [Header("Throwing Enemy Settings")]
    [SerializeField] private float MeleeDamage = 0;
    [SerializeField] private float RangeDamage = 0;
    [SerializeField] private float ProjectileSpeed = 0;
    [SerializeField] private Transform ShootPoint = null;
    [SerializeField] private GameObject ProjectilePrefab = null;
    [SerializeField] private float ReloadSpeed = 0;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip throwSound = null;

    private bool canShoot = true;

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
        if (canShoot)
        {
            Shoot(target.position);
        }
        float distance = (target.position - transform.position).magnitude;
        if (distance <= 3 && canDamage == true)
        {
            StartCoroutine(GiveDamageCoroutine());
        }
    }

    public void Shoot(Vector3 target)
    {
        IEnumerator ShootCoroutine()
        {
            canShoot = false;
            yield return new WaitForSeconds(ReloadSpeed);
            canShoot = true;
        }
        StartCoroutine(ShootCoroutine());
        if (Physics.Raycast(ShootPoint.position, (target - ShootPoint.position).normalized, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Player"))
            {
                audioSource.PlayOneShot(throwSound, 0.5f);
                GameObject projectile = Instantiate(ProjectilePrefab, ShootPoint.position, Quaternion.LookRotation((target - ShootPoint.position).normalized));
                projectile.GetComponent<Projectile>().Initialize((target - ShootPoint.position).normalized, RangeDamage, ProjectileSpeed);
            }
        }
    }
}
