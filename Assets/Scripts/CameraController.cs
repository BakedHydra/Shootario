using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private GameObject Player;

    private Vector3 previousPosition;
    private void Start()
    {
        previousPosition = Player.transform.position;
    }
    void Update()
    {
        this.transform.position += Player.transform.position - previousPosition;
        previousPosition = Player.transform.position;
    }
}
