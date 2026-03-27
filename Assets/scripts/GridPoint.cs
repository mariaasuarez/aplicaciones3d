using System.Collections.Generic;
using UnityEngine;

public class GridPoint : MonoBehaviour
{
    public int row;
    public int column;
    public GridPatternPuzzle puzzle;

    void OnTriggerEnter(Collider other)
    {
        // Aquí compruebas que el "other" sea la punta del ray/hand
        // Por ahora asumimos que cualquier cosa que entre cuenta.
        puzzle.TryAddPoint(this);
    }
}
