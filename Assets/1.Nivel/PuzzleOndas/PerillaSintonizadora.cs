using UnityEngine;
using UnityEngine.Events;

public class PerillaSintonizadora : MonoBehaviour
{
    [Header("Configuración de Giro")]
    public Transform objetoA_Girar; // El cilindro de la perilla
    public float sensibilidad = 2.0f;

    [Header("Evento de Salida")]
    public UnityEvent<float> OnValueChange; // Aquí conectaremos el audio

    private float valorActual = 0f;
    private Quaternion rotacionInicial;

    void Start()
    {
        rotacionInicial = objetoA_Girar.localRotation;
    }

    // Este método se ejecuta mientras el jugador agarra y mueve la perilla
    void Update()
    {
        // Calculamos el ángulo de rotación local en el eje Z (o el que uses)
        // Convertimos la rotación de -180/180 a un valor de 0 a 1
        float anguloZ = objetoA_Girar.localEulerAngles.z;

        if (anguloZ > 180) anguloZ -= 360; // Normalizar

        // Mapeamos el ángulo (ejemplo de -150 a 150 grados) a un valor 0-1
        valorActual = Mathf.InverseLerp(-150f, 150f, anguloZ);

        // Enviamos el valor al sintonizador de audio
        OnValueChange.Invoke(valorActual);
    }
}