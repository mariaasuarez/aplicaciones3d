using UnityEngine;
using UnityEngine.SceneManagement; // Importante para cambiar de escena

public class CambioEscena : MonoBehaviour
{
    // Esta función la llamaremos desde el botón
    public void CargarMapa(string nombreMapa)
    {
        SceneManager.LoadScene(nombreMapa);
    }
}