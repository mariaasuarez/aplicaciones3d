using UnityEngine;
using TMPro;

public class MemoriaCuadrosManager : MonoBehaviour
{
    [Header("Orden correcto")]
    public int[] ordenCorrecto = { 0, 1, 2, 3 };

    [Header("Estado actual")]
    public int pasoActual = 0;
    public bool juegoTerminado = false;

    [Header("Referencias")]
    public TMP_Text textoEstado;
    public GameObject premioFinal;
    public CuadroInteractivo[] cuadros;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoError;
    public AudioClip sonidoCompleto;

    void Start()
    {
        if (premioFinal != null)
            premioFinal.SetActive(false);

        ActualizarTexto("Observa el orden y presiona los cuadros correctamente");
        ReiniciarCuadros();
    }

    public void SeleccionarCuadro(int idCuadro)
    {
        if (juegoTerminado) return;

        if (idCuadro == ordenCorrecto[pasoActual])
        {
            CuadroInteractivo cuadro = BuscarCuadroPorID(idCuadro);
            if (cuadro != null)
                cuadro.MarcarCorrecto();

            if (audioSource != null && sonidoCorrecto != null)
                audioSource.PlayOneShot(sonidoCorrecto);

            pasoActual++;

            if (pasoActual >= ordenCorrecto.Length)
            {
                CompletarJuego();
            }
            else
            {
                ActualizarTexto("Correcto. Sigue con el siguiente cuadro");
            }
        }
        else
        {
            if (audioSource != null && sonidoError != null)
                audioSource.PlayOneShot(sonidoError);

            ActualizarTexto("Orden incorrecto. Intenta otra vez");
            ReiniciarSecuencia();
        }
    }

    void CompletarJuego()
    {
        juegoTerminado = true;
        ActualizarTexto("¡Muy bien! Completaste el recorrido multimedia");

        if (audioSource != null && sonidoCompleto != null)
            audioSource.PlayOneShot(sonidoCompleto);

        if (premioFinal != null)
            premioFinal.SetActive(true);
    }

    void ReiniciarSecuencia()
    {
        pasoActual = 0;
        ReiniciarCuadros();
    }

    void ReiniciarCuadros()
    {
        foreach (CuadroInteractivo cuadro in cuadros)
        {
            if (cuadro != null)
                cuadro.ResetearVisual();
        }
    }

    CuadroInteractivo BuscarCuadroPorID(int id)
    {
        foreach (CuadroInteractivo cuadro in cuadros)
        {
            if (cuadro != null && cuadro.idCuadro == id)
                return cuadro;
        }
        return null;
    }

    void ActualizarTexto(string mensaje)
    {
        if (textoEstado != null)
            textoEstado.text = mensaje;
    }
}