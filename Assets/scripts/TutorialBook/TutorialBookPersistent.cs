using UnityEngine;

/// <summary>
/// Hace que el libro tutorial persista entre escenas.
/// Ponlo en el mismo GameObject raíz (TutorialBook_Root).
/// Solo existirá UNA instancia durante toda la partida.
/// 
/// NOTA: La lógica de Singleton se movió a TutorialBookController.
/// Este script solo maneja el DontDestroyOnLoad.
/// </summary>
public class TutorialBookPersistent : MonoBehaviour
{
    void Awake()
    {
        // TutorialBookController ya maneja el Singleton.
        // Este script solo garantiza que el GameObject sobreviva al cambio de escena.
        DontDestroyOnLoad(gameObject);
    }
}