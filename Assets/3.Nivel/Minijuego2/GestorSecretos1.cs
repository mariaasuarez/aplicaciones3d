using UnityEngine;
using System.Collections;

public class GestorSecretos1 : MonoBehaviour
{
    [Header("Configuración de la Transición")]
    public float velocidadAparicion = 1.5f; // Segundos que tarda en revelarse

    [Header("Elementos UI a Revelar")]
    [Tooltip("Objetos con el componente Canvas Group (pueden ser textos, imágenes, etc.)")]
    public CanvasGroup[] elementosSecretos;

    void Start()
    {
        // Al empezar, ocultamos todo
        foreach (CanvasGroup elemento in elementosSecretos)
        {
            if (elemento != null)
            {
                elemento.alpha = 0;
                elemento.interactable = false;
                elemento.blocksRaycasts = false;
            }
        }
    }

    public void RevelarTodo()
    {
        StopAllCoroutines();
        StartCoroutine(EfectoAparecer());
    }

    IEnumerator EfectoAparecer()
    {
        float alphaActual = 0;
        while (alphaActual < 1)
        {
            alphaActual += Time.deltaTime / velocidadAparicion;
            foreach (CanvasGroup elemento in elementosSecretos)
            {
                if (elemento != null) elemento.alpha = alphaActual;
            }
            yield return null;
        }

        // Asegurar estado final
        foreach (CanvasGroup elemento in elementosSecretos)
        {
            if (elemento != null)
            {
                elemento.alpha = 1;
                elemento.interactable = true;
                elemento.blocksRaycasts = true;
            }
        }
    }
}