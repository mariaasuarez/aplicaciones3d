using UnityEngine;
using TMPro; // Necesario para usar el InputField de TextMeshPro
using UnityEngine.Events;

public class RadioPrincipal : MonoBehaviour
{
    [Header("Configuración del Código")]
    public string codigoCorrecto = "123"; // El número que debe coincidir
    public TMP_InputField campoDeTexto;   // Arrastra aquí tu InputField

    [Header("Acciones al Ganar")]
    public UnityEvent OnCodigoCorrecto;

    private bool yaGano = false;

    // Esta función se llamará cada vez que el texto cambie o al darle Enter
    public void VerificarCodigo()
    {
        if (yaGano) return;

        // Comparamos lo escrito con el código correcto
        if (campoDeTexto.text == codigoCorrecto)
        {
            Exito();
        }
    }

    void Exito()
    {
        yaGano = true;
        Debug.Log("¡Código Correcto! Pasando de nivel...");

        // Bloqueamos el campo para que no sigan escribiendo
        campoDeTexto.interactable = false;

        // Ejecutamos lo que quieras (abrir puerta, cambiar escena, etc.)
        OnCodigoCorrecto.Invoke();
    }
}