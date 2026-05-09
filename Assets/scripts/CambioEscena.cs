using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class CambioEscena : MonoBehaviour
{
    [Tooltip("Escribe aquí el nombre de la escena de destino")]
    public string nombreDeLaEscena = "NombreDeTuEscena";

    public void IrACinematica()
    {
        if (GestorCargasVR.Instancia != null)
        {
            GestorCargasVR.Instancia.CargarEscena(nombreDeLaEscena);
        }
        else
        {
            Debug.LogError("¡Error! No encontré el GestorCargasVR en la escena. Asegúrate de que el objeto existe.");

            //SceneManager.LoadScene(nombreDeLaEscena);
        }
    }
}