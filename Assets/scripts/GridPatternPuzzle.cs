using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pattern
{
    public List<Vector2Int> points; // (row, column) en orden
}

public class GridPatternPuzzle : MonoBehaviour
{
    [Header("Grid")]
    public int rows = 4;
    public int columns = 4;
    public List<GridPoint> gridPoints;

    [Header("Dibujo")]
    public LineRenderer linePrefab;
    private LineRenderer currentLine;
    private List<GridPoint> currentPath = new List<GridPoint>();

    [Header("Soluciones")]
    public List<Pattern> solutionPatterns;

    [Header("Resultado")]
    public GameObject keyFragmentPrefab;
    public Transform keySpawnPoint;

    private bool isDrawing = false;

    void Start()
    {
        // Opcional: puedes validar que gridPoints tenga rows*columns etc.
    }

    public void StartDraw()
    {
        if (isDrawing) return;
        isDrawing = true;
        currentPath.Clear();

        currentLine = Instantiate(linePrefab, transform);
        currentLine.positionCount = 0;
    }

    public void EndDraw()
    {
        if (!isDrawing) return;
        isDrawing = false;

        if (IsSolution(currentPath))
        {
            OnPuzzleSolved();
        }
        else
        {
            // Reset visual si quieres
            if (currentLine != null)
                Destroy(currentLine.gameObject);
        }
    }

    public void TryAddPoint(GridPoint point)
    {
        if (!isDrawing) return;
        if (currentPath.Contains(point)) return; // Evitar repetir

        currentPath.Add(point);

        if (currentLine != null)
        {
            currentLine.positionCount = currentPath.Count;
            currentLine.SetPosition(currentPath.Count - 1, point.transform.position);
        }
    }

    bool IsSolution(List<GridPoint> path)
    {
        if (path.Count == 0) return false;

        // Convertimos a lista de (row,column)
        List<Vector2Int> drawn = new List<Vector2Int>();
        foreach (var p in path)
        {
            drawn.Add(new Vector2Int(p.row, p.column));
        }

        foreach (var pattern in solutionPatterns)
        {
            if (pattern.points.Count != drawn.Count) continue;

            bool match = true;
            for (int i = 0; i < drawn.Count; i++)
            {
                if (drawn[i] != pattern.points[i])
                {
                    match = false;
                    break;
                }
            }

            if (match) return true;
        }
        return false;
    }

    void OnPuzzleSolved()
    {
        if (currentLine != null)
            currentLine.startColor = Color.green; // feedback simple

        if (keyFragmentPrefab != null && keySpawnPoint != null)
        {
            Instantiate(keyFragmentPrefab, keySpawnPoint.position, keySpawnPoint.rotation);
        }

        // Aquí puedes notificar al sistema de llaves o al manager del cuarto
    }
}