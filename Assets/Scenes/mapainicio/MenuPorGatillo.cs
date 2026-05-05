using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPorGatillo : MonoBehaviour
{
    // El objeto del menú que quieres mostrar/ocultar
    public GameObject menuObjeto;

    // La acción del botón (Bumper/Gatillo)
    public InputActionProperty botonBumper;

    void Start()
    {
        // Al empezar, nos aseguramos de que el menú esté oculto
        if (menuObjeto != null) menuObjeto.SetActive(false);
    }

    void Update()
    {
        // Si el botón está siendo presionado ahora mismo
        if (botonBumper.action.IsPressed())
        {
            if (!menuObjeto.activeSelf) menuObjeto.SetActive(true);
        }
        else // Si el botón NO está presionado (lo soltaste)
        {
            if (menuObjeto.activeSelf) menuObjeto.SetActive(false);
        }
    }

    private void OnEnable() => botonBumper.action.Enable();
    private void OnDisable() => botonBumper.action.Disable();
}