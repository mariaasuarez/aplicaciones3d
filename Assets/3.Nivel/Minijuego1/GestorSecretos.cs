using UnityEngine;

public class GestorSecretos : MonoBehaviour
{
    [Header("Objetos a Revelar")]
    public GameObject[] secretosOcultos;

    void Start()
    {
        // Al darle Play al juego, apaga todo para que estén escondidos
        OcultarTodo();
    }

    public void OcultarTodo()
    {
        foreach (GameObject secreto in secretosOcultos)
        {
            if (secreto != null)
            {
                secreto.SetActive(false); // Apaga el objeto
            }
        }
    }

    // El minijuego llamará a esta función cuando el jugador gane
    public void RevelarTodo()
    {
        foreach (GameObject secreto in secretosOcultos)
        {
            if (secreto != null)
            {
                secreto.SetActive(true); // Prende el objeto
            }
        }
    }
}