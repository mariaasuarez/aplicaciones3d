using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class LentesMagicos : MonoBehaviour
{
    [Header("Configuración Visual")]
    public GameObject filtroVR;
    public MeshRenderer meshCubo;

    [Header("Tipo de Lente")]
    [Tooltip("1 = Rojo, 2 = Verde, 3 = Azul")]
    [Range(1, 3)]
    public int colorDelFiltro = 1; // Eliges el color en el Inspector

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool lentesActivos = false;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.activated.AddListener(AlAccionar);
        grabInteractable.selectExited.AddListener(AlSoltar);
    }

    void Start()
    {
        if (filtroVR != null) filtroVR.SetActive(false);
        // Asegurarnos de que al iniciar el juego no haya ningún filtro activo en el Shader
        Shader.SetGlobalFloat("_FiltroGlobal", 0);
    }

    private void AlAccionar(ActivateEventArgs args)
    {
        lentesActivos = !lentesActivos;

        if (filtroVR != null) filtroVR.SetActive(lentesActivos);
        if (meshCubo != null) meshCubo.enabled = !lentesActivos;

        // MAGIA DEL SHADER: Si me pongo los lentes, mando el número de mi color. Si me los quito, mando un 0.
        if (lentesActivos)
        {
            Shader.SetGlobalFloat("_FiltroGlobal", colorDelFiltro);
        }
        else
        {
            Shader.SetGlobalFloat("_FiltroGlobal", 0);
        }
    }

    private void AlSoltar(SelectExitEventArgs args)
    {
        lentesActivos = false;
        if (filtroVR != null) filtroVR.SetActive(false);
        if (meshCubo != null) meshCubo.enabled = true;

        // Si suelto el cubo, apago la visión de rayos X en el mundo
        Shader.SetGlobalFloat("_FiltroGlobal", 0);
    }
}