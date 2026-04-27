using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    public PlayableDirector director;

    void Start()
    {
        director.stopped += AlTerminar;
    }

    void AlTerminar(PlayableDirector d)
    {
        GestorCargasVR.Instancia.CargarEscena("primermapa");
    }
}