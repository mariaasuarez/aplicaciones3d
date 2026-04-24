using UnityEngine;
using UnityEngine.Video;

public class PuzzleManager : MonoBehaviour
{
    [Header("Progreso")]
    public int piezasTotales = 4;
    private int contador = 0;

    [Header("Recompensa (Video)")]
    public GameObject pantallaPared;
    public VideoPlayer videoExplicativo;

    [Header("Efectos (Opcional)")]
    public AudioSource sonidoExito;

    void Start()
    {
        // Aseguramos que la pantalla esté apagada al inicio
        if (pantallaPared != null) pantallaPared.SetActive(false);
    }

    public void PiezaNuevaEncajada()
    {
        contador++;
        Debug.Log("Piezas encajadas: " + contador + "/" + piezasTotales);

        if (contador >= piezasTotales)
        {
            CompletarPuzzle();
        }
    }

    void CompletarPuzzle()
    {
        Debug.Log("¡Puzzle Completado!");

        if (pantallaPared != null) pantallaPared.SetActive(true);
        if (videoExplicativo != null) videoExplicativo.Play();
        if (sonidoExito != null) sonidoExito.Play();
    }
}