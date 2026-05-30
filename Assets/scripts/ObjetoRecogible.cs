using UnityEngine;
using UnityEngine.InputSystem;

public class ObjetoRecogible : MonoBehaviour
{
    [Header("Configuración")]
    public int idFragmento = 1;

    [Header("Referencias")]
    public GameObject avisoRecoger;

    [Header("Input")]
    public InputActionReference accionRecoger;

    private bool jugadorCerca = false;
    private bool yaRecogido = false;

    private void OnEnable()
    {
        if (accionRecoger != null)
        {
            accionRecoger.action.Enable();
            accionRecoger.action.performed += IntentarRecoger;
        }
    }

    private void OnDisable()
    {
        if (accionRecoger != null)
        {
            accionRecoger.action.performed -= IntentarRecoger;
        }
    }

    private void Start()
    {
        jugadorCerca = false;

        if (avisoRecoger != null)
            avisoRecoger.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (yaRecogido) return;

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            jugadorCerca = true;

            if (avisoRecoger != null)
                avisoRecoger.SetActive(true);

            Debug.Log("Jugador cerca de " + gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            jugadorCerca = false;

            if (avisoRecoger != null)
                avisoRecoger.SetActive(false);

            Debug.Log("Jugador lejos de " + gameObject.name);
        }
    }

    private void IntentarRecoger(InputAction.CallbackContext context)
    {
        if (yaRecogido) return;

        if (!jugadorCerca)
        {
            Debug.Log("No puedes recoger " + gameObject.name + " porque no estás cerca");
            return;
        }

        if (InventarioSistema.Instance != null)
        {
            InventarioSistema.Instance.RecogerFragmento(idFragmento);
        }
        else
        {
            Debug.LogWarning("No existe InventarioSistema en la escena.");
            return;
        }

        yaRecogido = true;

        if (avisoRecoger != null)
            avisoRecoger.SetActive(false);

        Debug.Log("Objeto recogido: " + gameObject.name);

        gameObject.SetActive(false);
    }
}