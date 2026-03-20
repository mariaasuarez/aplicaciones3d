using UnityEngine;
using UnityEngine.InputSystem;

public class InteractuableSilla : MonoBehaviour
{
    public Transform puntoDondeSentarse;
    public GameObject mensajeInteractuar;
    public float distanciaActivacion = 2.5f;

    private GameObject xrOrigin;
    private bool sentado = false;

    void Start()
    {
        // Buscamos el objeto que queremos mover
        xrOrigin = GameObject.Find("XR Origin (XR Rig)");
        if (mensajeInteractuar) mensajeInteractuar.SetActive(false);
    }

    void Update()
    {
        // Buscamos la cámara para medir distancia
        Camera cam = Camera.main;
        if (!cam || !xrOrigin) return;

        float distancia = Vector3.Distance(transform.position, cam.transform.position);

        if (distancia < distanciaActivacion && !sentado)
        {
            mensajeInteractuar.SetActive(true);
            if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
            {
                Sentarse();
            }
        }
        else
        {
            if (mensajeInteractuar) mensajeInteractuar.SetActive(false);
        }
    }

    void Sentarse()
    {
        if (xrOrigin && puntoDondeSentarse)
        {
            // USAMOS POSITION (Posición global) para ignorar escalas de padres
            xrOrigin.transform.position = puntoDondeSentarse.position;
            xrOrigin.transform.rotation = puntoDondeSentarse.rotation;

            // Forzamos a que la física no nos mueva
            Physics.SyncTransforms();

            sentado = true;
            Debug.Log("Teletransportado a: " + puntoDondeSentarse.position);
        }
    }
}