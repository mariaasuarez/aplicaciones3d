using UnityEngine;
using UnityEngine.SceneManagement; // Importante para cambiar escenas
using UnityEngine.XR.Interaction.Toolkit; // Importante para VR

public class CambioEscena : MonoBehaviour
{
    // Escribe aquí el nombre exacto de tu escena de la cinemática
    public string nombreDeLaEscena = "NombreDeTuEscena";

    public void IrACinematica()
    {
        // Esto carga la escena nueva
        SceneManager.LoadScene(nombreDeLaEscena);
    }
}