using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;

public class PressurePad : MonoBehaviour
{
    [Header("Pressure Pad Settings")]
    public bool isPressed { get; private set; }
    [SerializeField] private Material exampleMaterial;
    [SerializeField] private GameObject padPart1;
    [SerializeField] private GameObject padPart2;
    [SerializeField] private GameObject padPart3;
    [SerializeField] private GameObject padPart4;
    private Material material;

    private Color startColor;

    private void Start()
    {
        startColor = exampleMaterial.GetColor("_BaseColor");
        material = new Material(exampleMaterial);
        padPart1.GetComponent<Renderer>().material = material;
        padPart2.GetComponent<Renderer>().material = material;
        padPart3.GetComponent<Renderer>().material = material;
        padPart4.GetComponent<Renderer>().material = material;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Iteractable") || other.gameObject.CompareTag("Player"))
        {
            isPressed = true;
            material.SetColor("_BaseColor", Color.green);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Iteractable") || other.gameObject.CompareTag("Player"))
        {
            isPressed = false;
            material.SetColor("_BaseColor", startColor);
        }
    }
}
