using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscenaNuevo : MonoBehaviour
{
    public float tiempo = 5f; // duración de la cinemática

    void Start()
    {
        Invoke("IrAlJuego", tiempo);
    }

    void IrAlJuego()
    {
        SceneManager.LoadScene("MapaCinematica_final");
    }
}