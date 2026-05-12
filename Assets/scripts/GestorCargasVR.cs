using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorCargasVR : MonoBehaviour
{
    public static GestorCargasVR Instancia;

    [Header("UI de Carga")]
    public GameObject canvasCarga; 
    public RectTransform iconoAnimado; 
    public float velocidadGiro = -200f;

    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
            canvasCarga.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (canvasCarga.activeSelf && iconoAnimado != null)
        {
            iconoAnimado.Rotate(0f, 0f, velocidadGiro * Time.deltaTime);
        }
    }

    public void CargarEscena(string nombreEscena)
    {
        StartCoroutine(ProcesoDeCarga(nombreEscena));
    }

    IEnumerator ProcesoDeCarga(string nombreEscena)
    {
        yield return null;

        Camera camaraVR = Camera.main;
        if (camaraVR != null && canvasCarga != null)
        {
            canvasCarga.transform.SetParent(camaraVR.transform);
            canvasCarga.transform.localPosition = new Vector3(0, 0, 0.4f);
            canvasCarga.transform.localRotation = Quaternion.identity;
        }

        canvasCarga.SetActive(true);

        AsyncOperation operacion = SceneManager.LoadSceneAsync(nombreEscena);
        operacion.allowSceneActivation = false; 

        while (operacion.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        canvasCarga.transform.SetParent(this.transform);

        operacion.allowSceneActivation = true;

        yield return null;

        // Ocultar la pantalla de carga
        canvasCarga.SetActive(false);
    }
}