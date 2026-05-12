using System.Diagnostics;
using UnityEngine;

public class BotonSalir : MonoBehaviour
{
    public void SalirDelJuego()
    {
        // Usamos la ruta completa para evitar confusiones con otros scripts
        UnityEngine.Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}