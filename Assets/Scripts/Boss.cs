using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
public class Boss: Enemy
{
    [Header("Boss Enemy Settings")]
    [SerializeField] private float MeleeDamage = 0;
    [SerializeField] private float RangeDamage = 0;
    [SerializeField] private float ProjectileSpeed = 0;
    [SerializeField] private float ProjectilesCount = 0;
    [SerializeField] private float SpreadAngle = 0;
    [SerializeField] private float ReloadSpeed = 0;
    [SerializeField] private Transform ShootPoint = null;
    [SerializeField] private GameObject ProjectilePrefab = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip throwSound = null;

    private bool canShoot = true;

    private bool isStage1Acquired = false;
    private bool isStage2Acquired = false;
    private bool isStage3Acquired = false;

    public static event Action<string> SpawnStageEnemies;
    public static event Action BossHealthBarActive;
    public static event Action<float> BossHealthBarPercentile;

    private void Start()
    {
        FinalTrigger.enableMove += EnableMove;
        Health = MaxHealth;
        material = GetComponent<Renderer>().material;
        startColor = material.GetColor("_BaseColor");
        target = GameObject.FindWithTag("Player").transform;
        canMove = false;
        BossBarActivate();
        HealthBarControl();
    }

    private void BossBarActivate()
    {
        BossHealthBarActive?.Invoke();
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
        if (target == null) { return; }
        if (!canMove) { return; }
        if (canShoot)
        {
            Shoot(target.position);
        }
        float distance = (target.position - transform.position).magnitude;
        if (distance <= 6 && canDamage == true)
        {
            StartCoroutine(GiveDamageCoroutine());
        }
        Parent.transform.LookAt(target);
        HealthManager();
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
                float angleStep = SpreadAngle / (ProjectilesCount - 1);
                float startAngle = -SpreadAngle / 2;
                for (int i = 0; i < ProjectilesCount; i++)
                {
                    float CurrentAngle = startAngle + i * angleStep;
                    GameObject projectile = Instantiate(ProjectilePrefab, ShootPoint.position, Quaternion.LookRotation((target - ShootPoint.position).normalized) * Quaternion.Euler(0, CurrentAngle, 0));
                    projectile.GetComponent<Projectile>().Initialize((target - ShootPoint.position).normalized, RangeDamage, ProjectileSpeed);
                }
            }
        }
    }
    private void HealthManager()
    {
        if (Health <= MaxHealth / 4 && !isStage3Acquired)
        {
            Stage3();
        }
        else if (Health <= MaxHealth / 2 && !isStage2Acquired)
        {
            Stage2();
        }
        else if (Health <= MaxHealth / 4 * 3 && !isStage1Acquired)
        {
            Stage1();
        }
    }
    private void Stage1()
    {
        isStage1Acquired = true;
        ReloadSpeed = 4f;
        ProjectilesCount = 5;
        ProjectileSpeed += 2;
        SpawnStageEnemies?.Invoke("Stage1");

    }
    private void Stage2()
    {
        isStage2Acquired = true;
        ReloadSpeed = 3f;
        ProjectilesCount = 7;
        ProjectileSpeed += 2;
        SpawnStageEnemies?.Invoke("Stage2");
    }
    private void Stage3()
    {
        isStage3Acquired = true;
        ReloadSpeed = 2f;
        ProjectilesCount = 9;
        ProjectileSpeed += 2;
        SpawnStageEnemies?.Invoke("Stage3");
    }
    public override void GetKilled()
    {
        base.GetKilled();
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().WinUI();
    }

    public void EnableMove()
    {
        canMove = true;
        FinalTrigger.enableMove -= EnableMove;
    }

    private void HealthBarControl()
    {
        float percent = Health / MaxHealth;
        BossHealthBarPercentile?.Invoke(percent);
    }
    public override void GetDamage(float damage, RaycastHit hit)
    {
        IEnumerator GetDamageCoroutine()
        {
            material.SetColor("_BaseColor", Color.red);
            yield return new WaitForSeconds(0.25f);
            material.SetColor("_BaseColor", startColor);
        }
        Health -= damage;
        HealthBarControl();
        StartCoroutine(GetDamageCoroutine());
        if (Health <= 0)
        {
            GetKilled();
        }
    }
}

