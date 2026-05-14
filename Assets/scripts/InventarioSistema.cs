using UnityEngine;

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
    }

    private void Start()
    {
        ActualizarUI();
    }

    public void RecogerFragmento(int numeroFragmento)
    {
        if (numeroFragmento == 1)
            tieneFragmento1 = true;

        if (numeroFragmento == 2)
            tieneFragmento2 = true;

        if (numeroFragmento == 3)
            tieneFragmento3 = true;

        ActualizarUI();
    }

    public void ActualizarUI()
    {
        fragmentoUI1.SetActive(tieneFragmento1);
        fragmentoUI2.SetActive(tieneFragmento2);
        fragmentoUI3.SetActive(tieneFragmento3);
    }
}