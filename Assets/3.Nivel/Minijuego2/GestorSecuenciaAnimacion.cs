using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;

public class GestorSecuenciaAnimacion : MonoBehaviour
{
    [System.Serializable]
    public class DatosAnimacion
    {
        public string nombreAnimacion;
        public VideoClip clipVideo;
        [Tooltip("Las im�genes en el orden EXACTO de la animaci�n (0 a la �ltima)")]
        public Texture2D[] imagenesPoses;
    }
    [Header("Conexiones de Secretos")]
    public GestorSecretos gestorSecretos;

    [Header("Base de Datos de Animaciones")]
    public DatosAnimacion[] listaAnimaciones;

    [Header("Conexiones de la Escena")]
    public VideoPlayer reproductorVideo;
    [Tooltip("Los cubos f�sicos que est�n repartidos por la sala")]
    public MeshRenderer[] cubosFisicos;
    [Tooltip("Los sockets en orden de izquierda a derecha (1�, 2�, 3�, 4�, 5�)")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor[] socketsOrdenados;

    [Header("Configuración Sonora")]
    public AudioSource reproductorSonidos;
    public AudioClip sonidoAcierto;
    public AudioClip sonidoError;
    public AudioClip sonidoVictoria;

    private int animacionActualIndex;
    private bool nivelSuperado = false;

    void Start()
    {
        PrepararNivelAleatorio();
    }

    public void PrepararNivelAleatorio()
    {
        nivelSuperado = false;
        // Animacion al azar
        animacionActualIndex = Random.Range(0, listaAnimaciones.Length);
        DatosAnimacion animacionElegida = listaAnimaciones[animacionActualIndex];

        // Video en el muro
        reproductorVideo.clip = animacionElegida.clipVideo;
        reproductorVideo.Play();

        // Lista de las poses y se mezclan para repartirlas por los cubos los cubos
        List<int> indicesMezclados = new List<int>();
        for (int i = 0; i < animacionElegida.imagenesPoses.Length; i++)
        {
            indicesMezclados.Add(i);
        }
        indicesMezclados = indicesMezclados.OrderBy(x => Random.value).ToList();

        // Asigna una imagen aleatoria al material de cada cubo y guarda su ID real
        for (int i = 0; i < cubosFisicos.Length; i++)
        {
            int idRealDeLaPose = indicesMezclados[i];

            // Le cambia la textura principal al material del cubo
            cubosFisicos[i].material.mainTexture = animacionElegida.imagenesPoses[idRealDeLaPose];

            // Indica al script del cubo cu�l es su identidad real en esta ronda
            cubosFisicos[i].GetComponent<CuboPose>().idPoseCorrecta = idRealDeLaPose;
        }
    }

    // Esta función revisa el tablero cada vez que se pone un cubo
    public void EvaluarTablero()
    {
        if (nivelSuperado) return;

        bool hayAlgunError = false;
        int cubosCorrectos = 0;
        int cubosColocadosTotales = 0;

        foreach (var socket in socketsOrdenados)
        {
            if (socket.hasSelection) cubosColocadosTotales++;
        }

        for (int i = 0; i < socketsOrdenados.Length; i++)
        {
            UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable objetoEnSocket = socketsOrdenados[i].firstInteractableSelected;
            if (objetoEnSocket != null)
            {
                CuboPose cubo = objetoEnSocket.transform.GetComponent<CuboPose>();
                if (cubo != null && cubo.idPoseCorrecta == i) cubosCorrectos++;
                else hayAlgunError = true;
            }
        }

        if (hayAlgunError)
        {
            if (reproductorSonidos != null && sonidoError != null)
                reproductorSonidos.PlayOneShot(sonidoError);
        }
        else if (cubosCorrectos == socketsOrdenados.Length)
        {
            nivelSuperado = true;
            if (reproductorSonidos != null && sonidoVictoria != null)
                reproductorSonidos.PlayOneShot(sonidoVictoria);

            if (gestorSecretos != null)
            {
                gestorSecretos.RevelarTodo();
            }
        }
        else if (cubosColocadosTotales > 0)
        {
            if (reproductorSonidos != null && sonidoAcierto != null)
                reproductorSonidos.PlayOneShot(sonidoAcierto);
        }
    }
}