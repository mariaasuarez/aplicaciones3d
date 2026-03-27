using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class ControlCinematica : MonoBehaviour
{
    [Header("Cinemática")]
    [SerializeField] private PlayableDirector cinematica;

    [Header("XR Origin / Cámara")]
    [SerializeField] private GameObject xrRig;
    [SerializeField] private Camera mainXRCamera;

    [Header("Movimiento del Jugador")]
    [SerializeField] private LocomotionProvider locomotionMove;
    [SerializeField] private LocomotionProvider locomotionTurn;

    [Header("Opcional: otros scripts a bloquear")]
    [SerializeField] private MonoBehaviour[] scriptsABloquear;

    private bool cinematicaActiva = false;

    void Awake()
    {
        if (mainXRCamera == null && xrRig != null)
            mainXRCamera = xrRig.GetComponentInChildren<Camera>(true);
    }

    void Start()
    {
        if (cinematica == null)
        {
            Debug.LogError("ControlCinematica: No se asignó el PlayableDirector.");
            return;
        }

        if (mainXRCamera == null)
        {
            Debug.LogWarning("ControlCinematica: No se encontró la cámara XR. Verifica la referencia.");
        }

        IniciarCinematica();
    }

    public void IniciarCinematica()
    {
        cinematicaActiva = true;

        // La cámara XR NO se desactiva
        if (mainXRCamera != null && !mainXRCamera.gameObject.activeInHierarchy)
            mainXRCamera.gameObject.SetActive(true);

        // Bloquear locomoción
        if (locomotionMove != null) locomotionMove.enabled = false;
        if (locomotionTurn != null) locomotionTurn.enabled = false;

        // Bloquear otros scripts si hace falta
        if (scriptsABloquear != null)
        {
            foreach (var script in scriptsABloquear)
            {
                if (script != null)
                    script.enabled = false;
            }
        }

        cinematica.stopped -= CinematicaTerminada;
        cinematica.stopped += CinematicaTerminada;

        cinematica.time = 0;
        cinematica.Evaluate();
        cinematica.Play();
    }

    private void CinematicaTerminada(PlayableDirector director)
    {
        cinematicaActiva = false;

        // Restaurar locomoción
        if (locomotionMove != null) locomotionMove.enabled = true;
        if (locomotionTurn != null) locomotionTurn.enabled = true;

        // Reactivar otros scripts
        if (scriptsABloquear != null)
        {
            foreach (var script in scriptsABloquear)
            {
                if (script != null)
                    script.enabled = true;
            }
        }

        cinematica.stopped -= CinematicaTerminada;
    }
}