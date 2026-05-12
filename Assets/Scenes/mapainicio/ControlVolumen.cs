using UnityEngine;
using UnityEngine.Audio; // Necesario para el Mixer
using UnityEngine.UI;    // Necesario para el Slider

public class ControlVolumen : MonoBehaviour
{
    public AudioMixer miMixer;
    public Slider miSlider;

    void Start()
    {
        // Al empezar, ponemos el slider en el volumen actual
        float valorActual;
        miMixer.GetFloat("MasterVol", out valorActual);
        miSlider.value = valorActual;
    }

    public void CambiarVolumen(float valor)
    {
        // El volumen en Unity es logarítmico (-80dB a 0dB o 20dB)
        miMixer.SetFloat("MasterVol", valor);
    }
}