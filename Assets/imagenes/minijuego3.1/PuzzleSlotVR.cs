using UnityEngine;

public class PuzzleSlotVR : MonoBehaviour
{
    public int slotID;
    public Transform snapPoint;
    public PuzzleManagerVR puzzleManager;

    private bool occupied = false;

    private void OnTriggerEnter(Collider other)
    {
        if (occupied) return;

        PuzzlePieceVR piece = other.GetComponent<PuzzlePieceVR>();
        if (piece == null) return;
        if (piece.isPlaced) return;

        if (piece.pieceID == slotID)
        {
            occupied = true;
            piece.PlacePiece(snapPoint);
            puzzleManager.AddCorrectPiece();
        }
    }
}