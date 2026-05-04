using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PatronFigura
{
    public string nombreLetra;
    [Tooltip("Marca con un 'check' los espacios que DEBEN tener esfera")]
    public bool[] celdas = new bool[9];
}

public class ManagerPuzzleMatriz : MonoBehaviour
{
    [Header("Configuraci�n de Niveles")]
    [Tooltip("Las figuras en orden (ej. U, M, N, G)")]
    public List<PatronFigura> niveles;
    private int nivelActualIndex = 0;

    [Header("Conexiones de la Escena")]
    [Tooltip("Arrastra aqu� los 9 cubos que tienen el XR Socket Interactor")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor[] tableroSockets = new UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor[9];

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoAcierto;
    public AudioClip sonidoError;
    public AudioClip sonidoVictoriaFinal;

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
            UnityEngine.Debug.Log("�Completaste todas las letras!");
        }
        else
        {
            if (audioSource && sonidoAcierto) audioSource.PlayOneShot(sonidoAcierto);
            UnityEngine.Debug.Log("�Figura correcta! Siguiente letra: " + niveles[nivelActualIndex].nombreLetra);
        }
    }

    private void Fallar()
    {
        if (audioSource && sonidoError) audioSource.PlayOneShot(sonidoError);
        UnityEngine.Debug.Log("Patr�n incorrecto, revisa la figura.");
    }
}