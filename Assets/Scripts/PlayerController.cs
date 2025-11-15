using UnityEngine;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float Acceleration = 20f;
    [SerializeField] private float MaxSpeed = 10f;
    [SerializeField] private float JumpForce = 300f;
    [SerializeField] private float DashAcceleration = 100f;
    private bool OnGround = false;
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

    // Update is called once per frame
    void Update()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");

        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        if (Mathf.Max(Mathf.Abs(rigidbody.linearVelocity.x), Mathf.Abs(rigidbody.linearVelocity.z)) <= MaxSpeed)
        {
            rigidbody.AddForce(new Vector3(horizontalAxis * Acceleration, 0, verticalAxis * Acceleration), ForceMode.Force);
            Debug.Log(horizontalAxis.ToString());
        }
        else 
        {
            if (Mathf.Abs(rigidbody.linearVelocity.x) > MaxSpeed)
            {
                rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x - (rigidbody.linearVelocity.x - MaxSpeed), rigidbody.linearVelocity.y, rigidbody.linearVelocity.z);
            } 
            else
            if (Mathf.Abs(rigidbody.linearVelocity.z) > MaxSpeed)
            {
                rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, rigidbody.linearVelocity.y, rigidbody.linearVelocity.z - (rigidbody.linearVelocity.z - MaxSpeed));
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rigidbody.AddForce(new Vector3(horizontalAxis * DashAcceleration, 0, verticalAxis * DashAcceleration), ForceMode.Impulse);

        }

        if (OnGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }
        }
    }
}