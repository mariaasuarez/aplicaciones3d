using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PuzzlePieceVR : MonoBehaviour
{
    public int pieceID;
    public bool isPlaced = false;

    [HideInInspector] public XRGrabInteractable grabInteractable;
    [HideInInspector] public Rigidbody rb;

    private PuzzleSlotVR currentSlot = null;
    private bool wasSelectedLastFrame = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (grabInteractable == null || isPlaced) return;

        bool isCurrentlySelected = grabInteractable.isSelected;

        // Detecta el momento exacto en que el jugador suelta la pieza
        if (wasSelectedLastFrame && !isCurrentlySelected)
        {
            if (currentSlot != null)
            {
                currentSlot.TryPlacePiece(this);
            }
        }

        wasSelectedLastFrame = isCurrentlySelected;
    }

    public void SetCurrentSlot(PuzzleSlotVR slot)
    {
        currentSlot = slot;
    }

    public void PlacePiece(Transform targetPoint)
    {
        isPlaced = true;

        // Por seguridad, soltamos la pieza si todavía está agarrada
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            grabInteractable.interactionManager.SelectExit(
                grabInteractable.firstInteractorSelecting,
                grabInteractable
            );
        }

        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        currentSlot = null;
    }
}