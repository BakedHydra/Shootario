using System.Collections;
using Unity.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public string Name;
    public float Damage;
    public float FireRate;
    public float ClipSize;
    public float ReloadTime;
    public float BulletsRemaining { get; private set; }

    public Transform Pivot;
    public Transform FirePoint;

    public ParticleSystem MuzzleParticles;
    public AudioSource ShootSource;
    public AudioClip ShootSound;
    public AudioClip ReloadSound;
    public float ShootVolume = 0.5f;

    private bool canShoot = true;
    private bool isReloading = false;
    private bool isOnGround = true;
    private GameObject Player;

    private void Start() 
    { 
        if (Name == null)
        {
            Name = "TestWeapon";
        }
        if (Damage == 0)
        {
            Damage = 10f;
        }
        if (FireRate == 0)
        {
            FireRate = 300f;
        }
        if (ClipSize == 0)
        {
            ClipSize = 30f;
        }
        if (ReloadTime == 0) { ReloadTime = 3f; }
        BulletsRemaining = ClipSize;
        Player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isOnGround)
        {
            Take();
        }
    }

    public void Rotate(Vector3 lookPosition)
    {
        if (Pivot == null) { return; }
        Pivot.LookAt(lookPosition);
    }

    public void Shoot()
    {
        IEnumerator ShootCoroutine()
        {
            canShoot = false;
            yield return new WaitForSeconds(1 / (FireRate / 60));
            canShoot = true;
        }

        if (!canShoot) { return; }
        if (isReloading) { return; }
        if (MuzzleParticles != null)
        {
            MuzzleParticles.Play();
        }
        if (ShootSound != null)
        {
            ShootSource.PlayOneShot(ShootSound, ShootVolume);
        }
        if (Physics.Raycast(FirePoint.position, FirePoint.forward, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Enemy>().GetDamage(Damage, hit);
            }
            else if (hit.collider.CompareTag("Destroyable"))
            {
                hit.collider.GetComponent<Destroyable>().GetDamage(Damage, hit);
            }
        }
        if (BulletsRemaining > 0) 
        {
            StartCoroutine(ShootCoroutine());
        }
        BulletAmountControl();

    }
    private void BulletAmountControl()
    {
        IEnumerator ReloadCoroutine(float ReloadTime)
        {
            isReloading = true;
            ShootSource.PlayOneShot(ReloadSound, 1f);
            yield return new WaitForSeconds(ReloadTime);
            isReloading = false;
        }

        BulletsRemaining--;
        if (BulletsRemaining <= 0)
        {
            StartCoroutine(ReloadCoroutine(ReloadTime));
            BulletsRemaining = ClipSize;
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Take()
    {
        transform.position = Player.transform.GetChild(0).position;
        transform.rotation = Player.transform.rotation;
        transform.SetParent(Player.transform.GetChild(0));
        Player.GetComponent<PlayerController>().Weapon = this;
        Player.GetComponent<PlayerController>().GetWeapon(Name);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
}
