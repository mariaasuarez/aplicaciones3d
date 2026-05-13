using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

[System.Serializable]
public class PatronFigura
{
    public string nombreLetra;
    [Tooltip("Marca con un 'check' los espacios que DEBEN tener esfera")]
    public bool[] celdas = new bool[9];
}

public class ManagerPuzzleMatriz : MonoBehaviour
{
    [Header("Configuración de Niveles")]
    [Tooltip("Las figuras en orden (ej. U, M, N, G)")]
    public List<PatronFigura> niveles;
    private int nivelActualIndex = 0;

    [Header("Conexiones de la Escena")]
    [Tooltip("Arrastra aquí los 9 cubos que tienen el XR Socket Interactor")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor[] tableroSockets = new UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor[9];

    [Header("Interfaz de Usuario")]
    [Tooltip("Arrastra aquí el objeto de texto (TextMeshPro) que verá el usuario")]
    public TextMeshProUGUI textoGuiaLetra;
    public string prefijoTexto = "Figura a armar: ";

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoAcierto;
    public AudioClip sonidoError;
    public AudioClip sonidoVictoriaFinal;

    [Header("Efectos Mágicos (Recompensa)")]
    [Tooltip("El objeto que aparecerá al completar todas las figuras")]
    public GameObject recompensaObjeto;
    [Tooltip("Tiempo en segundos que tarda en aparecer")]
    public float duracionAparicion = 1.0f;
    [Tooltip("(Opcional) Partículas al ganar")]
    public GameObject particulasMagicasPrefab;

    [Header("Interacción Final (Cambio Escena)")]
    [Tooltip("El objeto/Canvas que contiene tu botón de 'Interactuar'")]
    public GameObject botonInteractuarUI;
    [Tooltip("Arrastra aquí la Main Camera de tu XR Origin")]
    public Transform jugadorVR;
    [Tooltip("El objeto que sirve como centro para el radio de interacción")]
    public Transform puntoDeInteraccion;
    [Tooltip("Radio en metros para que el botón aparezca")]
    public float radioDeInteraccion = 1.5f;

    [Header("Guía Visual (Brillo Destino)")]
    [Tooltip("El objeto que contiene la luz/partículas que brillará a lo lejos al ganar")]
    public GameObject efectoBrilloDestino;

    private Vector3 escalaOriginalRecompensa;
    private bool juegoTerminado = false;

    void Start()
    {
        if (recompensaObjeto != null)
        {
            escalaOriginalRecompensa = recompensaObjeto.transform.localScale;
            recompensaObjeto.SetActive(false);
        }
        if (botonInteractuarUI != null) botonInteractuarUI.SetActive(false);
        if (efectoBrilloDestino != null) efectoBrilloDestino.SetActive(false);

        ActualizarTextoPantalla();
    }

    void Update()
    {
        if (juegoTerminado && recompensaObjeto != null && recompensaObjeto.activeSelf && jugadorVR != null && puntoDeInteraccion != null)
        {
            float distancia = Vector3.Distance(jugadorVR.position, puntoDeInteraccion.position);

            if (distancia <= radioDeInteraccion)
            {
                if (!botonInteractuarUI.activeSelf) botonInteractuarUI.SetActive(true);
            }
            else
            {
                if (botonInteractuarUI.activeSelf) botonInteractuarUI.SetActive(false);
            }
        }
    }

    public void VerificarTablero()
    {
        if (nivelActualIndex >= niveles.Count) return;

        PatronFigura figuraActual = niveles[nivelActualIndex];
        bool esCorrecto = true;

        for (int i = 0; i < 9; i++)
        {
            bool tieneEsfera = tableroSockets[i].hasSelection;

            if (tieneEsfera != figuraActual.celdas[i])
            {
                esCorrecto = false;
                break;
            }
        }

        if (esCorrecto)
        {
            AvanzarNivel();
        }
        else
        {
            Fallar();
        }
    }

    private void AvanzarNivel()
    {
        nivelActualIndex++;

        if (nivelActualIndex >= niveles.Count)
        {
            if (audioSource && sonidoVictoriaFinal) audioSource.PlayOneShot(sonidoVictoriaFinal);
            if (textoGuiaLetra != null) textoGuiaLetra.text = "¡PUZZLE COMPLETADO!";
            UnityEngine.Debug.Log("¡Completaste todas las letras!");

            juegoTerminado = true;
            if (efectoBrilloDestino != null) efectoBrilloDestino.SetActive(true);

            if (recompensaObjeto != null)
            {
                StartCoroutine(AparecerObjetoMagicamente());
            }
        }
        else
        {
            if (audioSource && sonidoAcierto) audioSource.PlayOneShot(sonidoAcierto);
            ActualizarTextoPantalla();
            UnityEngine.Debug.Log("¡Figura correcta! Siguiente letra: " + niveles[nivelActualIndex].nombreLetra);
        }
    }

    private void ActualizarTextoPantalla()
    {
        if (textoGuiaLetra != null && nivelActualIndex < niveles.Count)
        {
            textoGuiaLetra.text = prefijoTexto + niveles[nivelActualIndex].nombreLetra;
        }
    }

    private void Fallar()
    {
        if (audioSource && sonidoError) audioSource.PlayOneShot(sonidoError);
        UnityEngine.Debug.Log("Patrón incorrecto, revisa la figura.");
    }

    private IEnumerator AparecerObjetoMagicamente()
    {
        if (particulasMagicasPrefab != null)
        {
            Instantiate(particulasMagicasPrefab, recompensaObjeto.transform.position, Quaternion.identity);
        }

        recompensaObjeto.transform.localScale = Vector3.zero;
        recompensaObjeto.SetActive(true);

        float tiempoPasado = 0f;
        while (tiempoPasado < duracionAparicion)
        {
            tiempoPasado += Time.deltaTime;
            float porcentaje = tiempoPasado / duracionAparicion;

            float crecimientoSuave = Mathf.SmoothStep(0f, 1f, porcentaje);

            recompensaObjeto.transform.localScale = Vector3.Lerp(Vector3.zero, escalaOriginalRecompensa, crecimientoSuave);

            yield return null;
        }

        recompensaObjeto.transform.localScale = escalaOriginalRecompensa;
    }
}