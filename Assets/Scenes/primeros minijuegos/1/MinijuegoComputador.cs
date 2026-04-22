using UnityEngine;
using UnityEngine.Video;

public class MinijuegoComputador : MonoBehaviour
{
    [Header("Referencias del Minijuego")]
    public GameObject quadVideo;
    public Light luzPantalla;

    [Header("Configuración del Botón")]
    public MeshRenderer rendererBoton; // Arrastra aquí la esfera (botón)
    public Material materialVerde;    // Arrastra aquí el material verde
    // No necesitamos el rojo en el script porque ya empieza rojo por defecto

    private bool yaEncendido = false;

    void Start()
    {
        quadVideo.SetActive(false);
        if (luzPantalla != null) luzPantalla.enabled = false;

        // El botón ya debería tener el material rojo puesto desde el editor
    }

    public void EncenderComputador()
    {
        if (!yaEncendido)
        {
            // 1. Activar Video y Luz
            quadVideo.SetActive(true);
            if (luzPantalla != null) luzPantalla.enabled = true;

            // 2. CAMBIAR COLOR DEL BOTÓN
            if (rendererBoton != null && materialVerde != null)
            {
                rendererBoton.material = materialVerde;
            }

            yaEncendido = true;
        }
    }
}