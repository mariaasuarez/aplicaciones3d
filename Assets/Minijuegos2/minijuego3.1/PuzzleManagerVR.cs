using UnityEngine;

public class PuzzleManagerVR : MonoBehaviour
{
    public int totalPieces = 3;
    private int correctPieces = 0;

    [Header("Llave")]
    public GameObject llaveObjeto; //la llave que YA está en la escena

    private bool llaveAparecio = false;

    private void Start()
    {
        //la llave debe estar apagada al iniciar
        if (llaveObjeto != null)
        {
            llaveObjeto.SetActive(false);
        }
    }

    public void AddCorrectPiece()
    {
        correctPieces++;
        Debug.Log("Piezas correctas: " + correctPieces + "/" + totalPieces);

        if (correctPieces >= totalPieces && !llaveAparecio)
        {
            ActivarLlave();
        }
    }

    void ActivarLlave()
    {
        llaveAparecio = true;

        if (llaveObjeto != null)
        {
            llaveObjeto.SetActive(true);
            Debug.Log("La llave apareció.");
        }
        else
        {
            Debug.LogWarning("No asignaste la llave en el inspector.");
        }
    }
}