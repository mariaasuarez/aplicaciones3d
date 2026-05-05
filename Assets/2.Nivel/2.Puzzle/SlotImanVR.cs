using UnityEngine;

public class SlotImanVR : MonoBehaviour
{
    [Tooltip("El punto central exacto donde la esfera debe imantarse")]
    public Transform puntoCentro;
    [Tooltip("Fuerza de atracción magnética")]
    public float fuerzaIman = 15f;

    [Header("Estado (No tocar)")]
    public bool tieneEsfera = false;

    private Rigidbody esferaRb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EsferaPuzzle") && !tieneEsfera)
        {
            tieneEsfera = true;
            esferaRb = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EsferaPuzzle") && other.GetComponent<Rigidbody>() == esferaRb)
        {
            tieneEsfera = false;
            esferaRb = null;
        }
    }

    private void FixedUpdate()
    {
        if (tieneEsfera && esferaRb != null)
        {
            Vector3 direccionHaciaCentro = puntoCentro.position - esferaRb.position;
            esferaRb.AddForce(direccionHaciaCentro * fuerzaIman);
        }
    }
}
