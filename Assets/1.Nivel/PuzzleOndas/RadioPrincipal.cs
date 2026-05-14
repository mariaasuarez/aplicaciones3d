using UnityEngine;
using TMPro; // Obligatorio para TextMeshPro
using UnityEngine.Events;

public class RadioPrincipal : MonoBehaviour
{
    [Header("Configuración del Código")]
    public string codigoCorrecto = "1509";
    public TMP_InputField campoDeTexto; // <-- Asegúrate que diga TMP_

    [Header("UI de Éxito")]
    public GameObject imagenVictoria;

    [Header("Acciones al Ganar")]
    public UnityEvent OnCodigoCorrecto;

    private bool yaGano = false;

    // Conecta esta función al evento On Value Changed del InputField
    public void VerificarCodigo()
    {
        if (yaGano) return;

        // Limpiamos espacios por si acaso y comparamos
        if (campoDeTexto.text.Trim() == codigoCorrecto)
        {
            Exito();
        }
    }

    void Exito()
    {
        yaGano = true;
        Debug.Log("¡LO LOGRASTE! Código correcto.");

        if (campoDeTexto != null)
            campoDeTexto.gameObject.SetActive(false);

        if (imagenVictoria != null)
            imagenVictoria.SetActive(true);

        OnCodigoCorrecto.Invoke();
    }
}