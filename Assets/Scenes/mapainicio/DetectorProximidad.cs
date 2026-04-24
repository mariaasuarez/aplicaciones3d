using UnityEngine;

public class DetectorProximidad : MonoBehaviour
{
    public GameObject botonImagen; // La imagen que dice "Sentarse"

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera")) botonImagen.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera")) botonImagen.SetActive(false);
    }
}