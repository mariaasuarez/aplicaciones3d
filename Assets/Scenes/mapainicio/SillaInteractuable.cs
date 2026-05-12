using UnityEngine;
using System.Collections;

public class SillaInteractuable : MonoBehaviour
{
    [Header("Configuración de Posición")]
    public GameObject xrOrigin;
    public Transform puntoSentado;

    [Header("Configuración de Menú")]
    public GameObject canvasMenu;
    public float tiempoEsperaMenu = 0.7f;
    public float velocidadFade = 1.5f; // Qué tan rápido aparece

    [Header("Bloqueo de Movimiento")]
    public GameObject locomotion;

    private CanvasGroup menuCanvasGroup;

    public void SentarUsuario()
    {
        // 1. Teletransporte y Bloqueo
        if (xrOrigin != null && puntoSentado != null)
        {
            xrOrigin.transform.position = puntoSentado.position;
            xrOrigin.transform.rotation = puntoSentado.rotation;
        }

        if (locomotion != null)
        {
            locomotion.SetActive(false);
        }

        // 2. Iniciar secuencia de aparición
        StartCoroutine(SecuenciaAparicionMenu());
    }

    IEnumerator SecuenciaAparicionMenu()
    {
        // Espera inicial para que el usuario se ubique
        yield return new WaitForSeconds(tiempoEsperaMenu);

        if (canvasMenu != null)
        {
            // Nos aseguramos de tener el Canvas Group
            menuCanvasGroup = canvasMenu.GetComponent<CanvasGroup>();
            if (menuCanvasGroup == null)
                menuCanvasGroup = canvasMenu.AddComponent<CanvasGroup>();

            // Empezamos transparente
            menuCanvasGroup.alpha = 0;
            canvasMenu.SetActive(true);

            // Efecto de Fade In
            float tiempo = 0;
            while (tiempo < 1)
            {
                tiempo += Time.deltaTime * velocidadFade;
                menuCanvasGroup.alpha = Mathf.Lerp(0, 1, tiempo);
                yield return null;
            }

            menuCanvasGroup.alpha = 1;
        }
    }
}