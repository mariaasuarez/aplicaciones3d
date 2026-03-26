using UnityEngine;

public class SintonizadorRadio : MonoBehaviour
{
    [Header("Fuentes de Audio")]
    public AudioSource audioEstatica; // Sonido de radio descompuesta (en Loop)
    public AudioSource audioVozLimpia; // El número secreto (en Loop)

    [Header("Configuración del Puzzle")]
    [Range(0, 1)] public float puntoExacto = 0.7f; // En qué parte de la perilla se escucha bien
    public float tolerancia = 0.1f; // Qué tan "difícil" es encontrar el punto

    private bool completado = false;

    // Este método lo llamará la perilla al girar (valor de 0 a 1)
    public void AjustarSintonizacion(float valorPerilla)
    {
        if (completado) return;

        // Calculamos qué tan cerca estamos del punto exacto
        // La diferencia será 0 si estamos justo encima
        float diferencia = Mathf.Abs(valorPerilla - puntoExacto);

        // 1. La estática baja si nos acercamos al punto
        audioEstatica.volume = Mathf.Clamp01(diferencia * 5);

        // 2. La voz limpia sube si nos acercamos al punto
        // Usamos una curva para que solo se escuche cuando estemos MUY cerca
        float volumenVoz = 1 - (diferencia / tolerancia);
        audioVozLimpia.volume = Mathf.Clamp01(volumenVoz);

        // Si el volumen de la voz es casi 1, lo damos por ganado
        if (volumenVoz >= 0.98f)
        {
            SintonizacionExitosa();
        }
    }

    void SintonizacionExitosa()
    {
        completado = true;
        audioEstatica.volume = 0;
        audioVozLimpia.volume = 1;
        // Aquí podrías cambiar el color de un foquito en la radio a verde
        Debug.Log("¡Señal captada!");
    }
}