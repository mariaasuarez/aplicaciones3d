using UnityEngine;

public class NavegadorMenu : MonoBehaviour
{
    [Header("Paneles del Menú")]
    public GameObject panelPrincipal;
    public GameObject panelAjustes;
    public GameObject panelAyuda;

    // Esta función se llama al empezar para que siempre inicie en el Principal
    void OnEnable()
    {
        IrAlPrincipal();
    }

    public void IrAAjustes()
    {
        DesactivarTodo();
        panelAjustes.SetActive(true);
    }

    public void IrAAyuda()
    {
        DesactivarTodo();
        panelAyuda.SetActive(true);
    }

    public void IrAlPrincipal()
    {
        DesactivarTodo();
        panelPrincipal.SetActive(true);
    }

    private void DesactivarTodo()
    {
        panelPrincipal.SetActive(false);
        panelAjustes.SetActive(false);
        panelAyuda.SetActive(false);
    }
}