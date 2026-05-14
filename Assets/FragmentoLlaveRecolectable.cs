using UnityEngine;
using UnityEngine.InputSystem;

public class FragmentoLlaveRecolectable : MonoBehaviour
{
    public int numeroFragmento = 1;

    private bool jugadorCerca = false;
    private bool recogido = false;

    private void Update()
    {
        if (!jugadorCerca || recogido) return;

        bool gatilloDerecho =
            Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;

        bool triggerVR =
            Gamepad.current != null && Gamepad.current.rightTrigger.wasPressedThisFrame;

        if (gatilloDerecho || triggerVR)
        {
            Recoger();
        }
    }

    private void Recoger()
    {
        recogido = true;

        Debug.Log("RECOGIÓ FRAGMENTO " + numeroFragmento);

        if (InventarioSistema.Instance != null)
        {
            InventarioSistema.Instance.RecogerFragmento(numeroFragmento);
        }
        else
        {
            Debug.LogWarning("No existe InventarioSistema en la escena.");
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            Debug.Log("Jugador cerca del fragmento");
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