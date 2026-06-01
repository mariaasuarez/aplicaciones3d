using UnityEngine;
using UnityEngine.InputSystem;

public class ObjetoRecogible : MonoBehaviour
{
    public int idFragmento = 1;
    public GameObject avisoRecoger;
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
            accionRecoger.action.performed -= IntentarRecoger;
    }

    private void Start()
    {
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
        }
    }

    private void IntentarRecoger(InputAction.CallbackContext context)
    {
        Debug.Log("Se oprimió recoger");

        if (!jugadorCerca)
        {
            Debug.Log("Estás oprimiendo recoger, pero no estás cerca.");
            return;
        }

        Recoger();
    }

    public void Recoger()
    {
        if (yaRecogido) return;

        if (InventarioSistema.Instance == null)
        {
            Debug.LogWarning("No existe InventarioSistema en la escena.");
            return;
        }

        InventarioSistema.Instance.RecogerFragmento(idFragmento);

        yaRecogido = true;

        if (avisoRecoger != null)
            avisoRecoger.SetActive(false);

        Debug.Log("Objeto recogido: " + gameObject.name);

        gameObject.SetActive(false);
    }
}