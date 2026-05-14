using UnityEngine;
using UnityEngine.InputSystem;

public class FragmentoLlaveRecolectable : MonoBehaviour
{
    public int numeroFragmento = 1;

    public InputActionProperty triggerDerecho;

    private bool jugadorCerca = false;

    void Update()
    {
        if (jugadorCerca)
        {
            if (triggerDerecho.action.WasPressedThisFrame())
            {
                RecogerFragmento();
            }
        }
    }

    void RecogerFragmento()
    {
        InventarioSistema.Instance.RecogerFragmento(numeroFragmento);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}