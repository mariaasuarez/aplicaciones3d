using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AutoCambioEscena : MonoBehaviour
{
    public float tiempoDeEspera = 16f; // Los segundos que dura tu cinemática
    public string nombreSiguienteEscena = "primermapa"; // ¡Cuidado con las mayúsculas!

    void Start()
    {
        // Esto arranca apenas aparece el objeto en la escena
        StartCoroutine(CuentaAtras());
    }

    IEnumerator CuentaAtras()
    {
        Debug.Log("Iniciando cuenta atrás para cambiar de escena...");

        // Espera el tiempo de la cinemática
        yield return new WaitForSeconds(tiempoDeEspera);

        Debug.Log("Cambiando a: " + nombreSiguienteEscena);

        // Cambia la escena
        SceneManager.LoadScene(nombreSiguienteEscena);
    }
}