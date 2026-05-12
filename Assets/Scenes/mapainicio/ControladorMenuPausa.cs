using UnityEngine;
using UnityEngine.InputSystem; // Necesario para los botones
using UnityEngine.XR.Interaction.Toolkit; // Necesario para VR

public class ControladorMenuPausa : MonoBehaviour
{
    [Header("Configuración del Menú")]
    public GameObject objetoMenu; // Arrastra aquí el Panel o Canvas del menú
    public InputActionProperty botonMostrarOcultar; // Aquí va la referencia al botón

    [Header("Ajustes de Posición")]
    public float distanciaFrenteAlUsuario = 1.5f;

    // ESTO ES VITAL: Activa la escucha del botón
    private void OnEnable()
    {
        botonMostrarOcultar.action.Enable();
    }

    // Desactiva la escucha al cerrar el juego o destruir el objeto
    private void OnDisable()
    {
        botonMostrarOcultar.action.Disable();
    }

    void Update()
    {
        // Detecta si presionaste el botón en este frame
        if (botonMostrarOcultar.action.WasPressedThisFrame())
        {
            AlternarMenu();
        }
    }

    public void AlternarMenu()
    {
        if (objetoMenu == null) return;

        bool estaActivo = !objetoMenu.activeSelf;
        objetoMenu.SetActive(estaActivo);

    }

   
}