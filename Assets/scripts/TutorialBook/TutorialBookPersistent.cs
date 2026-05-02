using UnityEngine;

/// <summary>
/// Hace que el libro tutorial persista entre escenas.
/// Ponlo en el mismo GameObject raíz (TutorialBook_Root).
/// Solo existirá UNA instancia durante toda la partida.
/// </summary>
public class TutorialBookPersistent : MonoBehaviour
{
    private static TutorialBookPersistent instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // elimina duplicado
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); // sobrevive al cambio de escena
    }
}
