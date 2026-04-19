using UnityEngine;

public class CuadroVR : MonoBehaviour
{
    [Tooltip("El ID del cuadro. 0, 1, 2 o 3.")]
    public int idDeEsteCuadro;

    [Tooltip("Arrastra el GameManager_Acertijos")]
    public MinijuegoAcertijos manager;

    // Se conecta al componente de VR
    public void AvisarAlManager()
    {
        if (manager != null)
        {
            manager.VerificarRespuesta(idDeEsteCuadro);
        }
    }
}