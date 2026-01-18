using System.Collections;
using System.ComponentModel;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float MoveSpeed = 0;
    [SerializeField] private float DashForce = 0;
    [SerializeField] private float DashCooldown = 0;
    [SerializeField] private Camera Camera = null;
    [SerializeField] private float Acceleration = 0;

    [DoNotSerialize] public Weapon Weapon = null;

    [Header("Player Health Settings")]
    [SerializeField] private float MaxHealth = 0;
    [DoNotSerialize] public bool canRegenerate = true;
    [SerializeField] private GameObject healthBar = null;
    [SerializeField] private TMP_Text healthBarText = null;

    [Header("Movement Audio Settings")]
    [SerializeField] private AudioClip MoveSound;
    [SerializeField] private AudioClip DashSound;
    [SerializeField] private AudioSource PlayerAudioSource;

    [Header("Ammo Amount Settings")]
    [SerializeField] private TMP_Text ammoText;
    public float CurrentHealth { get; private set; }

    private Rigidbody rb;
    private Transform tr;
    private const float MAXSPEED = 200f;
    private bool canUseDash = true;
    private bool regenerationCooldown = false;
    private bool canGetDamage = true;
    [DoNotSerialize] public bool CanMove = true;
    private bool isPistolAcquired = false;
    private bool isRifleAcquired = false;
    private bool isSniperAcquired = false;

    [DoNotSerialize] public Weapon Pistol;
    [DoNotSerialize] public Weapon Rifle;
    [DoNotSerialize] public Weapon Sniper;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = transform;

        if (DashCooldown == 0)
        {
            DashCooldown = 1000;
        }
        CurrentHealth = MaxHealth;
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            Movement();
            WeaponControl();
            HealthControl();
        }
        else
        {
            if (PlayerAudioSource.isPlaying)
            {
                PlayerAudioSource.Stop();
            }
        }
    }

    private void Movement()
    {
        float acceleration = Acceleration;
        float speed = MoveSpeed;
        float dashForce = DashForce;
        float verticalMove = Input.GetAxisRaw("Vertical");
        float horizontalMove = Input.GetAxisRaw("Horizontal");

        void SetMaxVelocity(float maxSpeed)
        {
            rb.linearVelocity = new Vector3(
                Mathf.Clamp(rb.linearVelocity.x, -maxSpeed, maxSpeed),
                Mathf.Clamp(rb.linearVelocity.y, -maxSpeed, maxSpeed),
                Mathf.Clamp(rb.linearVelocity.z, -maxSpeed, maxSpeed)
                );
        }

        void PlayAudio(AudioClip audioClip, bool priority)
        {
            if (priority)
            {
                PlayerAudioSource.Stop();
            }
            if (!PlayerAudioSource.isPlaying)
            {
                PlayerAudioSource.clip = audioClip;
                PlayerAudioSource.Play();
                return;
            }
        }

        void Running(float speed, float verticalMove, float horizontalMove)
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                tr.LookAt(new Vector3(hit.point.x, tr.position.y, hit.point.z));
            }
            if (verticalMove != 0 && rb.linearVelocity.magnitude <= speed)
            {
                rb.AddForce(verticalMove * acceleration * Vector3.forward);
                PlayAudio(MoveSound, false);
                SetMaxVelocity(speed);
            }
            if (horizontalMove != 0 && rb.linearVelocity.magnitude <= speed)
            {
                rb.AddForce(horizontalMove * acceleration * Vector3.right);
                PlayAudio(MoveSound, false);
                SetMaxVelocity(speed);
            }
            if (horizontalMove == 0 && verticalMove == 0)
            {
                PlayerAudioSource.Stop();
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            }
            SetMaxVelocity(MAXSPEED);
        }

        void Dashing()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && canUseDash)
            {
                StartCoroutine(DashCoroutine());
                rb.AddForce(tr.forward * dashForce, ForceMode.Impulse);
                PlayAudio(DashSound, true);
            }
        }

        IEnumerator DashCoroutine()
        {
            canUseDash = false;
            yield return new WaitForSeconds(DashCooldown);
            canUseDash = true;
        }

        Running(speed, verticalMove, horizontalMove);
        Dashing();
    }
    private void WeaponControl()
    {
        if (Weapon == null && (!isPistolAcquired || !isRifleAcquired || !isSniperAcquired))
        {
            return;
        }
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Weapon.Rotate(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Weapon.Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && Weapon != Pistol.GetComponent<Weapon>())
        {
            Weapon.Hide();
            Weapon = Pistol.GetComponent<Weapon>();
            Weapon.Show();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && Weapon != Rifle.GetComponent<Weapon>())
        {
            Weapon.Hide();
            Weapon = Rifle.GetComponent<Weapon>();
            Weapon.Show();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && Weapon != Sniper.GetComponent<Weapon>())
        {
            Weapon.Hide();
            Weapon = Sniper.GetComponent<Weapon>();
            Weapon.Show();
        }
        UpdateAmmoBar();
    }
    public void GetDamage(float Damage)
    {
        IEnumerator GetDamageCoroutine()
        {
            Color color = GetComponent<Renderer>().material.GetColor("_BaseColor");
            canGetDamage = false;
            GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
            yield return new WaitForSeconds(0.7f);
            GetComponent<Renderer>().material.SetColor("_BaseColor", color);
            canGetDamage = true;
        }
        if (canGetDamage)
        {
            StartCoroutine(GetDamageCoroutine());
            CurrentHealth -= Damage;
        }
    }
    public void HealthRegeneration()
    {
        IEnumerator HealthRegenerationCoroutine()
        {
            regenerationCooldown = true;
            CurrentHealth = Mathf.Min(CurrentHealth + 10, MaxHealth);
            yield return new WaitForSeconds(1);
            regenerationCooldown = false;
        }

        if (!regenerationCooldown)
        {
            StartCoroutine(HealthRegenerationCoroutine());
        }
    }
    public void GetWeapon(string WeaponName)
    {
        if (WeaponName == "Pistol")
        {
            isPistolAcquired = true;
            Pistol = Weapon;
        }
        if (WeaponName == "Rifle")
        {
            isRifleAcquired = true;
            Rifle = Weapon;
        }
        if (WeaponName == "Sniper")
        {
            isSniperAcquired = true;
            Sniper = Weapon;
        }
    }
    private void HealthControl()
    {
        UpdateHealthBar();
        if (CurrentHealth <= 0)
        {
            GameObject.FindWithTag("GameController").GetComponent<GameManager>().DeathUI();
            return;
        }
        if (CurrentHealth < MaxHealth && canRegenerate)
        {
            HealthRegeneration();
        }
    }
    private void UpdateHealthBar()
    {
        float percentage = CurrentHealth / MaxHealth;
        healthBar.transform.localScale = new Vector3(percentage, 1, 1);
        healthBarText.text = CurrentHealth.ToString();
    }

    private void UpdateAmmoBar()
    {
        ammoText.text = Weapon.BulletsRemaining.ToString();
    }
}

