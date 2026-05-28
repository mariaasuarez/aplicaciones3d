using UnityEngine;
using UnityEngine.SceneManagement;

public class InventarioSistema : MonoBehaviour
{
    public static InventarioSistema Instance;

    [Header("Fragmentos UI")]
    public GameObject fragmentoUI1;
    public GameObject fragmentoUI2;
    public GameObject fragmentoUI3;

    [Header("Estado")]
    public bool tieneFragmento1;
    public bool tieneFragmento2;
    public bool tieneFragmento3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        BuscarUIEnEscena();
        ActualizarUI();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BuscarUIEnEscena();
        ActualizarUI();
    }

    private void BuscarUIEnEscena()
    {
        fragmentoUI1 = GameObject.Find("FragmentoUI1");
        fragmentoUI2 = GameObject.Find("FragmentoUI2");
        fragmentoUI3 = GameObject.Find("FragmentoUI3");

        if (fragmentoUI1 == null) Debug.LogWarning("No encontré FragmentoUI1");
        if (fragmentoUI2 == null) Debug.LogWarning("No encontré FragmentoUI2");
        if (fragmentoUI3 == null) Debug.LogWarning("No encontré FragmentoUI3");
    }

    public void RecogerFragmento(int numeroFragmento)
    {
        Debug.Log("Recogiendo fragmento número: " + numeroFragmento);

        if (numeroFragmento == 1)
        {
            tieneFragmento1 = true;
        }
        else if (numeroFragmento == 2)
        {
            tieneFragmento2 = true;
        }
        else if (numeroFragmento == 3)
        {
            tieneFragmento3 = true;
        }
        else
        {
            Debug.LogWarning("Número de fragmento inválido: " + numeroFragmento);
            return;
        }

        ActualizarUI();
    }

    public void ActualizarUI()
    {
        if (fragmentoUI1 != null)
            fragmentoUI1.SetActive(tieneFragmento1);

        if (fragmentoUI2 != null)
            fragmentoUI2.SetActive(tieneFragmento2);

        if (fragmentoUI3 != null)
            fragmentoUI3.SetActive(tieneFragmento3);
    }
}