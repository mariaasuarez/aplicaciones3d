using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    [Header("UI de interacción")]
    public GameObject interactButtonObject;

    [Header("UI de diálogo")]
    public GameObject fullScreenDialogueUI;
    public TMP_Text dialogueText;

    [Header("Botón continuar")]
    public Button continueButton;

    [Header("Opciones")]
    public GameObject optionButton1;
    public GameObject optionButton2;
    public GameObject optionButton3;

    public TMP_Text optionText1;
    public TMP_Text optionText2;
    public TMP_Text optionText3;

    [Header("Textos del NPC")]
    [TextArea(2, 5)]
    public string initialDialogue;

    [TextArea(2, 5)]
    public string finalDialogue;

    [Header("Opciones del jugador")]
    public string option1;
    public string option2;
    public string option3;

    [Header("Respuestas del NPC")]
    [TextArea(2, 5)] public string response1;
    [TextArea(2, 5)] public string response2;
    [TextArea(2, 5)] public string response3;

    private bool playerInRange = false;
    private int dialogueState = 0;

    private void Start()
    {
        interactButtonObject.SetActive(false);
        fullScreenDialogueUI.SetActive(false);

        HideOptions();

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(ContinueDialogue);
    }

    private int playerCollidersInside = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("ENTRÓ EL PLAYER: " + other.name);

        playerCollidersInside++;

        playerInRange = true;
        interactButtonObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("SALIO EL PLAYER: " + other.name);

        playerCollidersInside--;

        if (playerCollidersInside <= 0)
        {
            playerCollidersInside = 0;
            playerInRange = false;
            interactButtonObject.SetActive(false);
            CloseDialogue();
        }
    }

    // 🔥 BOTÓN INTERACTUAR
    public void Interact()
    {
        if (!playerInRange) return;

        interactButtonObject.SetActive(false);
        fullScreenDialogueUI.SetActive(true);

        dialogueText.text = initialDialogue;

        HideOptions();
        continueButton.gameObject.SetActive(true);

        dialogueState = 1;
    }

    public void ContinueDialogue()
    {
        if (dialogueState == 1)
        {
            dialogueText.text = "";
            continueButton.gameObject.SetActive(false);
            ShowOptions();
            dialogueState = 2;
        }
        else if (dialogueState == 3)
        {
            dialogueText.text = finalDialogue;
            continueButton.gameObject.SetActive(true);
            dialogueState = 4;
        }
        else if (dialogueState == 4)
        {
            CloseDialogue();
        }
    }

    public void ChooseOption(int option)
    {
        HideOptions();

        if (option == 1)
            dialogueText.text = response1;
        else if (option == 2)
            dialogueText.text = response2;
        else if (option == 3)
            dialogueText.text = response3;

        continueButton.gameObject.SetActive(true);
        dialogueState = 3;
    }

    private void ShowOptions()
    {
        optionButton1.SetActive(true);
        optionButton2.SetActive(true);
        optionButton3.SetActive(true);

        optionText1.text = option1;
        optionText2.text = option2;
        optionText3.text = option3;
    }

    private void HideOptions()
    {
        optionButton1.SetActive(false);
        optionButton2.SetActive(false);
        optionButton3.SetActive(false);
    }

    public void CloseDialogue()
    {
        fullScreenDialogueUI.SetActive(false);
        HideOptions();
        dialogueState = 0;

        if (playerInRange)
            interactButtonObject.SetActive(true);
    }
}