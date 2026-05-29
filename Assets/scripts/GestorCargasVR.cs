using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

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
        StartCoroutine(ProcesoDeCargaDirecta(nombreEscena));
    }

    IEnumerator ProcesoDeCargaDirecta(string nombreEscena)
    {
        Camera camaraCarga = Camera.main;
        if (camaraCarga != null && canvasCarga != null)
        {
            canvasCarga.transform.SetParent(camaraCarga.transform);
            canvasCarga.transform.localPosition = new Vector3(0, 0, 0.4f);
            canvasCarga.transform.localRotation = Quaternion.identity;
        }

        if (canvasCarga != null) canvasCarga.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        if (canvasCarga != null) canvasCarga.transform.SetParent(this.transform);

        // 1. Carga de mapa
        SceneManager.LoadScene(nombreEscena);

        // 2. Esperamos a que la escena cargue y el hardware XR se conecte
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.2f);

        // 3. BUSCAMOS AL JUGADOR
        GameObject xrOriginObj = GameObject.Find("XR Origin (XR Rig)");

        if (xrOriginObj != null)
        {
            // Borrar memoria física de las gafas
            List<XRInputSubsystem> subsistemasXR = new List<XRInputSubsystem>();
            SubsystemManager.GetSubsystems<XRInputSubsystem>(subsistemasXR);
            foreach (var subsistema in subsistemasXR)
            {
                subsistema.TryRecenter();
            }

            CharacterController cc = xrOriginObj.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            XROrigin origenXR = xrOriginObj.GetComponent<XROrigin>();

            if (origenXR != null)
            {
                // Calculamos la altura para que tus pies queden en Y: 0
                float alturaCabeza = origenXR.CameraInOriginSpaceHeight;

                origenXR.MoveCameraToWorldLocation(new Vector3(0, alturaCabeza, 0));
                origenXR.MatchOriginUpCameraForward(Vector3.up, Vector3.forward);

                UnityEngine.Debug.Log("[ÉXITO] Jugador centrado a la altura correcta.");
            }
            else
            {
                // Respaldo
                xrOriginObj.transform.position = Vector3.zero;
                xrOriginObj.transform.rotation = Quaternion.identity;
            }

            if (cc != null) cc.enabled = true;
        }
        else
        {
            UnityEngine.Debug.Log("[ERROR] No se encontró 'XR Origin (XR Rig)'.");
        }

        if (canvasCarga != null) canvasCarga.SetActive(false);
    }
}
