using UnityEngine;

public class MusicaNivel : MonoBehaviour
{
    [Header("Configuración de Audio")]
    public AudioClip musicaDeEsteMapa; // Aquí arrastras el archivo de audio (.mp3, .wav)
    [Range(0f, 1f)]
    public float volumen = 0.5f;
    public bool bucle = true; // Para que la música no se detenga

    private AudioSource source;

    void Start()
    {
        // Creamos un AudioSource automáticamente si no existe
        source = gameObject.AddComponent<AudioSource>();

        // Configuramos el sonido
        source.clip = musicaDeEsteMapa;
        source.volume = volumen;
        source.loop = bucle;
        source.playOnAwake = true;

        // Si tenemos un Audio Mixer (el que creamos para el Slider de ajustes)
        // debemos asignarlo aquí para que el volumen del menú funcione
        // source.outputAudioMixerGroup = miMixerGroup; 

        source.Play();
    }
}