using UnityEngine;


public class PuzzlePieceVR : MonoBehaviour
{
    public int pieceID;
    public bool isPlaced = false;

    [HideInInspector] public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    [HideInInspector] public Rigidbody rb;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    public void PlacePiece(Transform targetPoint)
    {
        isPlaced = true;

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
    }
}