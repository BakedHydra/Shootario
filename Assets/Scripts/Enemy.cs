using Mono.Cecil;
using System.Collections;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float MaxHealth = 0;
    public float Damage = 0;
    public float Health { get; private set; }
    [SerializeField] private GameObject parent;
    [SerializeField] private bool CanShoot = false;
    [SerializeField] private Transform ShootPoint = null;
    [SerializeField] private GameObject ProjectilePrefab = null;
    [SerializeField] private float ReloadSpeed;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip throwSound = null;

    private NavMeshAgent navMeshAgent;
    private bool canMove;
    private Material material;
    private Transform Target = null;
    private bool canDamage = true;
    private Color startColor;
    private void Start()
    {
        if (MaxHealth == 0)
        {
            MaxHealth = 100;
        }
        if (Damage == 0)
        {
            Damage = 50;
        }
        Health = MaxHealth;
        navMeshAgent = GetComponent<NavMeshAgent>();
        material = GetComponent<Renderer>().material;
        startColor = material.GetColor("_BaseColor");
        canMove = true;
        Target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        IEnumerator GiveDamageCoroutine()
        {
            canDamage = false;
            Target.gameObject.GetComponent<PlayerController>().GetDamage(Damage);
            yield return new WaitForSeconds(1.5f);
            canDamage = true;   
        }
        if (!canMove) { return; }
        if (navMeshAgent == null) { return; }
        if (Target == null) { return; }
        navMeshAgent.SetDestination(Target.position);
        if (CanShoot)
        {
            Shoot(Target.transform.position);
        }
        float distance = (Target.position - transform.position).magnitude;
        if (distance <= 3 && canDamage == true)
        {
            StartCoroutine(GiveDamageCoroutine());
        }

    }

    public void GetDamage(float damage, RaycastHit hit)
    {
        void GetKilled()
        {
            Destroy(parent);
        }

        IEnumerator GetDamageCoroutine()
        {
            material.SetColor("_BaseColor", Color.red);
            navMeshAgent.enabled = false;
            canMove = false;
            GetComponent<Rigidbody>().isKinematic = false;
            Vector3 hitVector = (hit.point - Target.position).normalized;
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
    private void Shoot(Vector3 target)
    {
        IEnumerator ShootCoroutine()
        {
            CanShoot = false;
            yield return new WaitForSeconds(ReloadSpeed);
            CanShoot = true;
        }
        StartCoroutine(ShootCoroutine());
        audioSource.PlayOneShot(throwSound, 0.5f);
        if (Physics.Raycast(ShootPoint.position, (target - ShootPoint.position).normalized, out RaycastHit hit)) 
        {
            if (hit.transform.CompareTag("Player"))
            {
                GameObject projectile = Instantiate(ProjectilePrefab, ShootPoint.position, Quaternion.LookRotation((target - ShootPoint.position).normalized));
                projectile.GetComponent<Projectile>().Initialize((target - ShootPoint.position).normalized);
            }
        }
    }
}
