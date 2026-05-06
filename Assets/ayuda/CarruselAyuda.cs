using UnityEngine;
using UnityEngine.UI;

public class CarruselAyuda : MonoBehaviour
{
    [Header("Componentes de UI")]
    public Image slotIzquierdo;
    public Image slotDerecho;

    [Header("Colección de Imágenes")]
    // Aquí pondrás las 6 imágenes de la izquierda y las 6 de la derecha
    public Sprite[] paginasIzquierda;
    public Sprite[] paginasDerecha;

    private int indiceActual = 0;

    void OnEnable()
    {
        indiceActual = 0;
        ActualizarPareja();
    }

    public void SiguientePareja()
    {
        // Cambia al siguiente par (0 a 5)
        indiceActual = (indiceActual + 1) % paginasIzquierda.Length;
        ActualizarPareja();
    }

    public void AnteriorPareja()
    {
        indiceActual--;
        if (indiceActual < 0) indiceActual = paginasIzquierda.Length - 1;
        ActualizarPareja();
    }

    private void ActualizarPareja()
    {
        // Verificamos que las listas tengan el mismo tamaño para evitar errores
        if (paginasIzquierda.Length == paginasDerecha.Length)
        {
            slotIzquierdo.sprite = paginasIzquierda[indiceActual];
            slotDerecho.sprite = paginasDerecha[indiceActual];
        }
    }
}