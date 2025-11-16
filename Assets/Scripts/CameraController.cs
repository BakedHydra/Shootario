using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private float Speed = 10f;

    private Vector3 camera_position;
    private Vector3 player_position;
    private Vector3 camera_offset;
    private Vector3 center_position;
    private float eps = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera_offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        camera_position = transform.position;
        player_position = PlayerTransform.position;
        float distance = Vector3.Distance(camera_position - camera_offset, player_position);

        if (distance > eps)
        {
            transform.position = Vector3.Lerp(camera_position, player_position + camera_offset, (Mathf.Pow(Speed, distance)) * Time.deltaTime);
        }
    }
}
