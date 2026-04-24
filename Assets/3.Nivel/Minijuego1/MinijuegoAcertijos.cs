using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

public class MinijuegoAcertijos : MonoBehaviour
{
    [System.Serializable]
    public class DatosCuadro
    {
        public string nombrePersonaje;
        public string[] acertijos = new string[2];
    }

    [Header("Configuración Visual")]
    public TMPro.TMP_Text textoPantallaPrincipal;

    [Header("Configuración Sonora")]
    public AudioSource audioSourceManager;
    public AudioClip sonidoAcierto;
    public AudioClip sonidoVictoria;
    public AudioClip sonidoError;

    [Header("Dificultad")]
    public int maxFallosPermitidos = 3; // Límite de errores
    private int fallosActuales = 0;

    [Header("Conexiones Extra")]
    public GestorSecretos gestorDeSecretos;

    [Header("Base de Datos (Orden: 0, 1, 2, 3)")]
    public DatosCuadro[] cuadrosData = new DatosCuadro[4];

    private struct Ronda
    {
        public int idCorrecto;
        public string textoAcertijo;
    }

    private List<Ronda> rondasDelJuego = new List<Ronda>();
    private int rondaActual = 0;
    private bool juegoTerminado = false; // Evita que el juego siga si ya gano o perdio

    void Start()
    {
        PrepararJuego();
    }

    void PrepararJuego()
    {
        rondasDelJuego.Clear();
        rondaActual = 0;
        fallosActuales = 0;
        juegoTerminado = false;

        for (int i = 0; i < cuadrosData.Length; i++)
        {
            int acertijoAleatorio = Random.Range(0, 2);

            Ronda nuevaRonda = new Ronda();
            nuevaRonda.idCorrecto = i;
            nuevaRonda.textoAcertijo = cuadrosData[i].acertijos[acertijoAleatorio];

            rondasDelJuego.Add(nuevaRonda);
        }

        rondasDelJuego = rondasDelJuego.OrderBy(x => Random.value).ToList();
        ActualizarPantalla();
    }

    void ActualizarPantalla()
    {
        if (rondaActual < rondasDelJuego.Count)
        {
            textoPantallaPrincipal.text = rondasDelJuego[rondaActual].textoAcertijo;
        }
        else
        {
            textoPantallaPrincipal.text = "¡Felicidades! Has resuelto todos los acertijos.";
            juegoTerminado = true;
            if (audioSourceManager != null && sonidoVictoria != null)
                audioSourceManager.PlayOneShot(sonidoVictoria);

            if (gestorDeSecretos != null)
            {
                gestorDeSecretos.RevelarTodo();
            }
        }
    }

    public void VerificarRespuesta(int idCuadroTocado)
    {
        // Si el juego ya terminó, no hacer nada aunque toque los cuadros
        if (juegoTerminado) return;

        if (idCuadroTocado == rondasDelJuego[rondaActual].idCorrecto)
        {
            // acerto
            if (audioSourceManager != null && sonidoAcierto != null)
                audioSourceManager.PlayOneShot(sonidoAcierto);

            rondaActual++;
            ActualizarPantalla();
        }
        else
        {
            // fallo
            fallosActuales++;

            if (audioSourceManager != null && sonidoError != null)
                audioSourceManager.PlayOneShot(sonidoError);

            if (fallosActuales >= maxFallosPermitidos)
            {
                textoPantallaPrincipal.text = "Lo siento pero has superado el límite máximo de intentos, puedes volver a intentarlo después.";
                juegoTerminado = true; // Bloquea el juego
            }
        }
    }
}