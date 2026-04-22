using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PiezaLogic : MonoBehaviour
{
    [Header("Referencias")]
    public Transform puntoDeAgarre;  // El empty que creaste DENTRO de la pieza
    public Transform miAnclaje;     // El empty que creaste en la MESA

    [Header("Configuración")]
    public float distanciaPermitida = 0.08f; // Margen de error para encajar

    private bool encajada = false;
    private XRGrabInteractable grab;
    private PuzzleManager manager;

    void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
        manager = FindObjectOfType<PuzzleManager>();

        // Detectar automáticamente cuando sueltas la pieza con la mano VR
        grab.selectExited.AddListener(AlSoltar);
    }

    void AlSoltar(SelectExitEventArgs args)
    {
        if (encajada) return;

        // Comparamos la posición GLOBAL del punto de agarre con la del anclaje
        float distancia = Vector3.Distance(puntoDeAgarre.position, miAnclaje.position);

        if (distancia < distanciaPermitida)
        {
            EncajarEnSuSitio();
        }
    }

    void EncajarEnSuSitio()
    {
        encajada = true;

        // 1. Guardamos la escala actual
        Vector3 escalaReal = transform.lossyScale;

        // 2. Desactivamos físicas y agarre
        grab.enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
        }

        // 3. LA MAGIA: Usamos el Punto de Agarre como "ancla" temporal
        // Primero, hacemos que el Punto de Agarre sea el PADRE de la pieza por un segundo
        Transform punto = puntoDeAgarre; // El hijo que sí está centrado
        Vector3 posicionOriginalPunto = punto.position;
        Quaternion rotacionOriginalPunto = punto.rotation;

        // Movemos el Punto de Agarre al Anclaje de la mesa
        punto.position = miAnclaje.position;
        punto.rotation = miAnclaje.rotation;

        // Calculamos cuánto se movió el punto y aplicamos ese MISMO movimiento a la pieza
        Vector3 diferenciaPos = punto.position - posicionOriginalPunto;
        transform.position += diferenciaPos;

        // Ajustamos la rotación para que coincida
        transform.rotation = miAnclaje.rotation * Quaternion.Inverse(punto.localRotation);

        // 4. Liberamos de cualquier padre y forzamos escala
        transform.SetParent(null);
        transform.localScale = escalaReal;

        if (GetComponent<Collider>()) GetComponent<Collider>().enabled = false;

        manager.PiezaNuevaEncajada();
        Debug.Log("Pieza alineada usando el hijo como referencia central.");
    }
}