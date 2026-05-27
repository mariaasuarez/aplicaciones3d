using UnityEngine;

public class CuadroInteractivo : MonoBehaviour
{
    [Header("Identificación")]
    public int idCuadro;

    [Header("Manager")]
    public MemoriaCuadrosManager manager;

    [Header("Visual")]
    public Renderer cuadroRenderer;
    public Material materialNormal;
    public Material materialCorrecto;

    private bool yaMarcado = false;

    public void Interactuar()
    {
        if (yaMarcado) return;
        if (manager == null) return;

        manager.SeleccionarCuadro(idCuadro);
    }

    public void MarcarCorrecto()
    {
        yaMarcado = true;

        if (cuadroRenderer != null && materialCorrecto != null)
            cuadroRenderer.material = materialCorrecto;
    }

    public void ResetearVisual()
    {
        yaMarcado = false;

        if (cuadroRenderer != null && materialNormal != null)
            cuadroRenderer.material = materialNormal;
    }
}