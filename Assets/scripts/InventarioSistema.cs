using UnityEngine;
using UnityEngine.UI;

public class InventarioSistema : MonoBehaviour
{
    [Header("UI Fragmentos")]
    public GameObject imagenFragmento1;
    public GameObject imagenFragmento2;
    public GameObject imagenFragmento3;

    [Header("Estado del inventario")]
    public bool tieneFragmento1 = false;
    public bool tieneFragmento2 = false;
    public bool tieneFragmento3 = false;

    private void Start()
    {
        ActualizarUI();
    }

    public void RecogerFragmento(int idFragmento)
    {
        switch (idFragmento)
        {
            case 1:
                tieneFragmento1 = true;
                break;
            case 2:
                tieneFragmento2 = true;
                break;
            case 3:
                tieneFragmento3 = true;
                break;
            default:
                Debug.LogWarning("ID de fragmento no válido: " + idFragmento);
                return;
        }

        ActualizarUI();
        Debug.Log("Se recogió el fragmento: " + idFragmento);
    }

    private void ActualizarUI()
    {
        if (imagenFragmento1 != null)
            imagenFragmento1.SetActive(tieneFragmento1);

        if (imagenFragmento2 != null)
            imagenFragmento2.SetActive(tieneFragmento2);

        if (imagenFragmento3 != null)
            imagenFragmento3.SetActive(tieneFragmento3);
    }
}