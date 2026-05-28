using UnityEngine;
using UnityEngine.InputSystem;

public class NavegadorMenu : MonoBehaviour
{
    [Header("Paneles del Menú")]
    public GameObject panelPrincipal;
    public GameObject panelAjustes;
    public GameObject panelAyuda;

    [Header("Configuración de Entrada")]
    [Tooltip("Asigna aquí la acción mapeada al Botón Y (Secondary Button) del control izquierdo")]
    public InputActionReference alternarMenuAction;

    void Start()
    {
        OcultarMenu();
    }

    void Update()
    {
        // Mantiene el soporte para pruebas en PC con la tecla M
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

    private void OnAlternarMenu(InputAction.CallbackContext context)
    {
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