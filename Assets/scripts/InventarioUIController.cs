using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventarioUIController : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelInventario;
    public Image iconoInventario;

    [Header("Input")]
    public InputActionReference abrirInventarioAction;

    private bool inventarioAbierto = false;

    private void OnEnable()
    {
        if (abrirInventarioAction != null)
        {
            abrirInventarioAction.action.Enable();
            abrirInventarioAction.action.performed += ToggleInventario;
            Debug.Log("Acción de inventario habilitada");
        }
        else
        {
            Debug.LogWarning("No se asignó abrirInventarioAction");
        }
    }

    private void OnDisable()
    {
        if (abrirInventarioAction != null)
        {
            abrirInventarioAction.action.performed -= ToggleInventario;
            abrirInventarioAction.action.Disable();
        }
    }

    private void Start()
    {
        if (panelInventario != null)
        {
            panelInventario.SetActive(false);
            Debug.Log("Panel inventario oculto al inicio");
        }
        else
        {
            Debug.LogWarning("No se asignó panelInventario");
        }
    }

    private void ToggleInventario(InputAction.CallbackContext context)
    {
        Debug.Log("Se ejecutó ToggleInventario");

        inventarioAbierto = !inventarioAbierto;

        if (panelInventario != null)
        {
            panelInventario.SetActive(inventarioAbierto);
            Debug.Log("Inventario abierto: " + inventarioAbierto);
        }
    }
}