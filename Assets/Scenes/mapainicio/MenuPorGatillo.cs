using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggleVR : MonoBehaviour
{
    [Header("Configuración del Menú")]
    public GameObject menuObjeto;

    [Header("Acción de Entrada")]
    [Tooltip("Asigna aquí la acción 'Menu' o 'MenuButton' del control izquierdo")]
    public InputActionProperty botonMenu;

    void Start()
    {
        // Aseguramos que el menú empiece oculto
        if (menuObjeto != null)
            menuObjeto.SetActive(false);
    }

    void Update()
    {
        // 'WasPressedThisFrame' detecta solo el momento inicial del clic, 
        // no importa cuánto tiempo dejes presionado el botón después.
        if (botonMenu.action.WasPressedThisFrame())
        {
            AlternarMenu();
        }
    }

    void AlternarMenu()
    {
        if (menuObjeto != null)
        {
            // El símbolo '!' invierte el estado actual:
            // Si está activo (true), lo vuelve falso. Si está falso, lo vuelve true.
            bool nuevoEstado = !menuObjeto.activeSelf;
            menuObjeto.SetActive(nuevoEstado);

            // Tip profesional: Si quieres que el menú aparezca frente al jugador
            // podrías añadir aquí el código para posicionarlo.
        }
    }

    private void OnEnable() => botonMenu.action.Enable();
    private void OnDisable() => botonMenu.action.Disable();
}