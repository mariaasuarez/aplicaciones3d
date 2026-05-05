using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BotonMemoriaVR : MonoBehaviour
{
    [HideInInspector]
    public Texture2D imagenAsignada; // El botón recordará qué imagen tiene puesta

    [Header("Conexiones Visuales")]
    public MeshRenderer meshRenderer;
    public Material materialNormal;
    public Material materialSeleccionado;

    private bool estaSeleccionado = false;
    private MinijuegoMemoriaManager manager;

    void Awake()
    {
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        manager = FindObjectOfType<MinijuegoMemoriaManager>();
        ResetearBoton();
    }

    public void AsignarImagen(Texture2D nuevaImagen)
    {
        imagenAsignada = nuevaImagen;
        if (meshRenderer != null && meshRenderer.material != null)
        {
            meshRenderer.material.mainTexture = nuevaImagen;
        }
    }

    public void AlPulsarBoton()
    {
        if (estaSeleccionado) DesmarcarBoton();
        else
        {
            if (manager != null && manager.IntentarSeleccionarBoton(imagenAsignada))
            {
                MarcarBoton();
            }
        }
    }

    public void MarcarBoton()
    {
        estaSeleccionado = true;
        if (materialSeleccionado != null)
        {
            meshRenderer.material = materialSeleccionado;
            meshRenderer.material.mainTexture = imagenAsignada;
        }
    }

    public void DesmarcarBoton()
    {
        estaSeleccionado = false;
        if (materialNormal != null)
        {
            meshRenderer.material = materialNormal;
            meshRenderer.material.mainTexture = imagenAsignada;
        }

        if (manager != null) manager.EliminarSeleccion(imagenAsignada);
    }

    public void ResetearBoton()
    {
        DesmarcarBoton();
    }
}
