using UnityEngine;

public class PuzzleManagerVR : MonoBehaviour
{
    public int totalPieces = 3;
    private int correctPieces = 0;

    [Header("Llave")]
    public GameObject llavePrefab;
    public Transform spawnLlave;

    private bool llaveAparecio = false;

    public void AddCorrectPiece()
    {
        correctPieces++;
        Debug.Log("Piezas correctas: " + correctPieces + "/" + totalPieces);

        if (correctPieces >= totalPieces && !llaveAparecio)
        {
            SpawnKey();
        }
    }

    void SpawnKey()
    {
        llaveAparecio = true;

        if (llavePrefab != null && spawnLlave != null)
        {
            Instantiate(llavePrefab, spawnLlave.position, spawnLlave.rotation);
        }
        else
        {
            Debug.LogWarning("Falta asignar el prefab de la llave o el punto de aparición.");
        }
    }
}