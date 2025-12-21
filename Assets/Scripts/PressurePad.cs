using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;

public class PressurePad : MonoBehaviour
{
    [DoNotSerialize] public bool isPressed;
    [SerializeField] private Material padMaterial;
    private Color startColor;

    private void Start()
    {
        startColor = padMaterial.GetColor("_BaseColor");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Iteractable") || collision.gameObject.CompareTag("Player"))
        {
            isPressed = true;
            padMaterial.SetColor("_BaseColor", Color.green);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Iteractable") || collision.gameObject.CompareTag("Player"))
        {
            isPressed = false;
            padMaterial.SetColor("_BaseColor", startColor);
        }
    }
}
