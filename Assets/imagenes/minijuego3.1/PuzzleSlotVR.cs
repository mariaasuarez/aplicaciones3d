using UnityEngine;

public class PuzzleSlotVR : MonoBehaviour
{
    public int slotID;
    public Transform snapPoint;
    public PuzzleManagerVR puzzleManager;

    private bool occupied = false;
    private PuzzlePieceVR currentPieceInside;

    private void OnTriggerEnter(Collider other)
    {
        if (occupied) return;

        PuzzlePieceVR piece = other.GetComponent<PuzzlePieceVR>();
        if (piece == null) return;
        if (piece.isPlaced) return;

        // Solo aceptamos la pieza correcta
        if (piece.pieceID == slotID)
        {
            currentPieceInside = piece;
            piece.SetCurrentSlot(this);
            Debug.Log("Pieza correcta dentro del slot " + slotID);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PuzzlePieceVR piece = other.GetComponent<PuzzlePieceVR>();
        if (piece == null) return;

        if (piece == currentPieceInside)
        {
            currentPieceInside = null;
            piece.SetCurrentSlot(null);
            Debug.Log("Pieza salió del slot " + slotID);
        }
    }

    public void TryPlacePiece(PuzzlePieceVR piece)
    {
        if (occupied) return;
        if (piece == null) return;
        if (piece.isPlaced) return;

        // Seguridad extra
        if (piece.pieceID != slotID) return;
        if (piece != currentPieceInside) return;

        occupied = true;
        piece.PlacePiece(snapPoint);
        puzzleManager.AddCorrectPiece();

        Debug.Log("Pieza colocada correctamente en slot " + slotID);
    }
}