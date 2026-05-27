using UnityEngine;
using UnityEngine.InputSystem;

public class NavegadorMenu : MonoBehaviour
{
    [Header("Paneles del Menú")]
    public GameObject panelPrincipal;
    public GameObject panelAjustes;
    public GameObject panelAyuda;

    [Header("Configuración de Entrada")]
    public InputActionReference alternarMenuAction;

    // Al iniciar el juego, decidimos si arranca oculto o visible
    void Start()
    {
        // Si quieres que el menú empiece CERRADO al darle Play, descomenta la línea de abajo:
        OcultarMenu();
    }
    void Update()
    {
        // Si presionas la tecla M en el teclado del computador
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool estaVisible = panelPrincipal.activeSelf || panelAjustes.activeSelf || panelAyuda.activeSelf;
            if (estaVisible) OcultarMenu();
            else IrAlPrincipal();
        }
    }

    void OnEnable()
    {
        if (alternarMenuAction != null)
        {
            alternarMenuAction.action.Enable();
            alternarMenuAction.action.performed += OnAlternarMenu;
        }
    }

    void OnDisable()
    {
        if (alternarMenuAction != null)
        {
            alternarMenuAction.action.performed -= OnAlternarMenu;
        }
    }

    // Se ejecuta al presionar el gatillo
    private void OnAlternarMenu(InputAction.CallbackContext context)
    {
        // Revisamos si alguno de los paneles está visible actualmente
        bool estaVisible = panelPrincipal.activeSelf || panelAjustes.activeSelf || panelAyuda.activeSelf;

        if (estaVisible)
        {
            OcultarMenu();
        }
        else
        {
            IrAlPrincipal();
        }
    }

    public void IrAAjustes()
    {
        DesactivarTodosLosPaneles();
        panelAjustes.SetActive(true);
    }

    public void IrAAyuda()
    {
        DesactivarTodosLosPaneles();
        panelAyuda.SetActive(true);
    }

    public void IrAlPrincipal()
    {
        DesactivarTodosLosPaneles();
        panelPrincipal.SetActive(true);
    }

    // En lugar de apagar el objeto con el script, solo apagamos los paneles visuales
    public void OcultarMenu()
    {
        DesactivarTodosLosPaneles();
    }

    private void DesactivarTodosLosPaneles()
    {
        if (panelPrincipal != null) panelPrincipal.SetActive(false);
        if (panelAjustes != null) panelAjustes.SetActive(false);
        if (panelAyuda != null) panelAyuda.SetActive(false);
    }
}