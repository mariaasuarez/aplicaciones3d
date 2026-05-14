using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FragmentoLlaveRecolectable : MonoBehaviour
{
    public int numeroFragmento = 1;

    private XRGrabInteractable grab;
    private bool yaRecogido = false;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grab.selectEntered.AddListener(RecogerFragmento);
        grab.hoverEntered.AddListener(JugadorApuntando);
    }

    private void OnDisable()
    {
        grab.selectEntered.RemoveListener(RecogerFragmento);
        grab.hoverEntered.RemoveListener(JugadorApuntando);
    }

    private void JugadorApuntando(HoverEnterEventArgs args)
    {
        Debug.Log("Jugador está apuntando/tocando la llave");
    }

    private void RecogerFragmento(SelectEnterEventArgs args)
    {
        if (yaRecogido) return;

        yaRecogido = true;

        Debug.Log("Llave recogida");

        InventarioSistema.Instance.RecogerFragmento(numeroFragmento);

        Destroy(gameObject);
    }
}