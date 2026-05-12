using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    [Header("Componentes")]
    public PlayableDirector director;

    [Header("Configuración de Destino")]
    [Tooltip("Escribe aquí el nombre exacto de la escena a la que quieres ir")]
    public string nombreDeLaEscena = "primermapa";

    void Start()
    {
        if (director == null)
        {
            director = GetComponent<PlayableDirector>();
        }

        if (director != null)
        {
            director.stopped += AlTerminar;
        }
    }

    void AlTerminar(PlayableDirector d)
    {
        GestorCargasVR.Instancia.CargarEscena(nombreDeLaEscena);
    }
}