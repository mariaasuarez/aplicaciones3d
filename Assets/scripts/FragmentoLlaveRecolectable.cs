using UnityEngine;
using UnityEngine.InputSystem;

public class FragmentoLlaveRecolectable : MonoBehaviour
{
    public int numeroFragmento = 1;

    private bool jugadorCerca = false;
    private bool recogido = false;
    private bool triggerPresionadoAntes = false;

    private void Update()
    {
        if (!jugadorCerca || recogido) return;

        bool triggerDerechoAhora = false;

        UnityEngine.XR.InputDevice manoDerecha =
            UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand);

        if (manoDerecha.isValid)
        {
            manoDerecha.TryGetFeatureValue(
                UnityEngine.XR.CommonUsages.triggerButton,
                out triggerDerechoAhora
            );
        }

        bool teclaG = Keyboard.current != null &&
                      Keyboard.current.gKey.wasPressedThisFrame;

        if ((triggerDerechoAhora && !triggerPresionadoAntes) || teclaG)
        {
            Recoger();
        }

        triggerPresionadoAntes = triggerDerechoAhora;
    }

    private void Recoger()
    {
        recogido = true;

        Debug.Log("RECOGIÓ FRAGMENTO " + numeroFragmento);

        if (InventarioSistema.Instance != null)
        {
            InventarioSistema.Instance.RecogerFragmento(numeroFragmento);
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
            triggerPresionadoAntes = false;
        }
    }
}