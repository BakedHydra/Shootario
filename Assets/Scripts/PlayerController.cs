using UnityEngine;
using Unity.Mathematics;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float Acceleration = 20f;
    [SerializeField] private float MaxSpeed = 10f;
    [SerializeField] private float MaxFallSpeed = 100f;
    [SerializeField] private float JumpForce = 300f;
    [SerializeField] private float DashAcceleration = 150f;

    private bool OnGround = false;
    private bool DashAvailable = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
       OnGround = true; 
    }

    private void OnTriggerExit(Collider other)
    {
        OnGround = false;
    }

    void Update()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");

        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

        if (horizontalAxis != 0 || verticalAxis != 0) 
        {
            if (rigidbody.linearVelocity.magnitude <= MaxSpeed)
            {
                rigidbody.AddForce(new Vector3(horizontalAxis * Acceleration, 0, verticalAxis * Acceleration), ForceMode.Force);
            }
            else 
            {
                rigidbody.AddForce(-rigidbody.linearVelocity.normalized * 2, ForceMode.Force);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //rigidbody.AddForce(new Vector3(horizontalAxis * DashAcceleration, 0, verticalAxis * DashAcceleration), ForceMode.Impulse);
            Dash(rigidbody, new Vector3(horizontalAxis, 0, verticalAxis), DashAcceleration, 1);
        }

        if (OnGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }
        }

        rigidbody.linearVelocity = new Vector3(
            Mathf.Clamp(rigidbody.linearVelocity.x, -MaxSpeed * 3, MaxSpeed * 3),
            rigidbody.linearVelocity.y,
            //Mathf.Clamp(rigidbody.linearVelocity.y, -MaxFallSpeed, MaxFallSpeed),
            Mathf.Clamp(rigidbody.linearVelocity.z, -MaxSpeed * 3, MaxSpeed * 3)
            );
        Debug.Log(rigidbody.linearVelocity.ToString());
    }
    //IEnumerator Dash(Rigidbody rigidbody, Vector3 Direction, float DashAcceleration)
    //{
    //    rigidbody.AddForce(Direction * DashAcceleration, ForceMode.Impulse);
    //    yield return new WaitForSeconds(5);

    //}

    IEnumerator CooldownRoutine(float Cooldown)
    {
        yield return new WaitForSeconds(Cooldown);
        DashAvailable = true;
    }

    private void Dash(Rigidbody rigidbody, Vector3 Direction, float DashAcceleration, float Cooldown)
    {
        if (DashAvailable == true)
        {
            rigidbody.AddForce(Direction * DashAcceleration, ForceMode.Impulse);
            DashAvailable = false;
            StartCoroutine(CooldownRoutine(Cooldown));

        }
    }
}