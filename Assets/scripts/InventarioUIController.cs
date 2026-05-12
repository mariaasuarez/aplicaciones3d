using UnityEngine;
using UnityEngine.UI;

public class InventarioUIController : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelInventario;
    public Image iconoInventario;

    private void Start()
    {
        if (panelInventario != null)
        {
            panelInventario.SetActive(true);
            Debug.Log("Inventario fijo visible arriba");
        }
        else
        {
            Debug.LogWarning("No se asignó panelInventario");
        }
    }

    public void MostrarIcono(Sprite nuevoIcono)
    {
        if (iconoInventario != null)
        {
            iconoInventario.sprite = nuevoIcono;
            iconoInventario.enabled = true;
        }
    }

    public void OcultarIcono()
    {
        if (iconoInventario != null)
        {
            iconoInventario.enabled = false;
        }
    }
}