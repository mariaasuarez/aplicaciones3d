using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class RadioPrincipal : MonoBehaviour
{
    [Header("Configuración del Código")]
    public string codigoCorrecto = "1509";
    public TMP_InputField campoDeTexto;

    [Header("UI de Éxito")]
    public GameObject imagenVictoria;

    [Header("Acciones al Ganar")]
    public UnityEvent OnCodigoCorrecto;


    [Header("Efectos Mágicos (Recompensa)")]
    [Tooltip("El objeto que aparecerá al completar el código")]
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

    private bool yaGano = false;
    private Vector3 escalaOriginalRecompensa;

    void Start()
    {
        if (botonInteractuarUI != null) botonInteractuarUI.SetActive(false);
        if (efectoBrilloDestino != null) efectoBrilloDestino.SetActive(false);

        if (recompensaObjeto != null)
        {
            escalaOriginalRecompensa = recompensaObjeto.transform.localScale;
            recompensaObjeto.SetActive(false);
        }
    }

    void Update()
    {
        if (yaGano && recompensaObjeto != null && recompensaObjeto.activeSelf && jugadorVR != null && puntoDeInteraccion != null)
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

    public void VerificarCodigo()
    {
        if (yaGano) return;

        if (campoDeTexto.text.Trim() == codigoCorrecto)
        {
            Exito();
        }
    }

    void Exito()
    {
        yaGano = true;
        Debug.Log("¡LO LOGRASTE! Código correcto.");

        if (campoDeTexto != null)
            campoDeTexto.gameObject.SetActive(false);

        if (imagenVictoria != null)
            imagenVictoria.SetActive(true);

        if (efectoBrilloDestino != null)
        {
            efectoBrilloDestino.SetActive(true);
        }

        if (recompensaObjeto != null)
        {
            StartCoroutine(AparecerObjetoMagicamente());
        }

        OnCodigoCorrecto.Invoke();
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