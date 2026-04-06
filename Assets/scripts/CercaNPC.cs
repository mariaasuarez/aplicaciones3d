using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    [Header("UI de interacción")]
    public GameObject interactButtonObject;

    [Header("UI de diálogo en pantalla completa")]
    public GameObject fullScreenDialogueUI;
    public TMP_Text dialogueText;
    public Image npcImage;

    [Header("Contenido del NPC")]
    [TextArea(3, 6)]
    public string npcMessage = "Hola. Ve al punto marcado y recoge el objeto.";
    public Sprite npcSprite;

    private bool playerInRange = false;
    private bool isDialogueOpen = false;

    private void Start()
    {
        interactButtonObject.SetActive(false);
        fullScreenDialogueUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo entró al trigger");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Es el jugador");
            playerInRange = true;
            interactButtonObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactButtonObject.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!playerInRange) return;

        isDialogueOpen = true;

        interactButtonObject.SetActive(false);
        fullScreenDialogueUI.SetActive(true);

        dialogueText.text = npcMessage;

        if (npcImage != null && npcSprite != null)
            npcImage.sprite = npcSprite;
    }

    public void CloseDialogue()
    {
        isDialogueOpen = false;
        fullScreenDialogueUI.SetActive(false);

        if (playerInRange)
            interactButtonObject.SetActive(true);
    }


}