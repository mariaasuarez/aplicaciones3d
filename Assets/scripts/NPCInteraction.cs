using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    [Header("Canvas de ESTE NPC")]
    public GameObject canvasNPC;

    [Header("UI Interactuar")]
    public GameObject interactButtonObject;

    [Header("UI Diálogo")]
    public GameObject dialoguePanel;
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

    [Header("Textos")]
    public string initialDialogue;
    public string finalDialogue;

    public string option1;
    public string option2;
    public string option3;

    public string response1;
    public string response2;
    public string response3;

    private bool playerInRange = false;
    private int dialogueState = 0;
    private int playerCollidersInside = 0;

    void Start()
    {
        canvasNPC.SetActive(false);
        interactButtonObject.SetActive(false);
        dialoguePanel.SetActive(false);
        HideOptions();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerCollidersInside++;
        playerInRange = true;
        interactButtonObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerCollidersInside--;

        if (playerCollidersInside <= 0)
        {
            playerCollidersInside = 0;
            playerInRange = false;
            CloseDialogue();
        }
    }

    public void Interact()
    {
        Debug.Log("Interactuando con: " + gameObject.name);

        if (!playerInRange) return;

        canvasNPC.SetActive(true);
        interactButtonObject.SetActive(false);
        dialoguePanel.SetActive(true);

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

        if (option == 1) dialogueText.text = response1;
        if (option == 2) dialogueText.text = response2;
        if (option == 3) dialogueText.text = response3;

        continueButton.gameObject.SetActive(true);
        dialogueState = 3;
    }

    void ShowOptions()
    {
        optionButton1.SetActive(true);
        optionButton2.SetActive(true);
        optionButton3.SetActive(true);

        optionText1.text = option1;
        optionText2.text = option2;
        optionText3.text = option3;
    }

    void HideOptions()
    {
        optionButton1.SetActive(false);
        optionButton2.SetActive(false);
        optionButton3.SetActive(false);
    }

    void CloseDialogue()
    {
        canvasNPC.SetActive(false);
        dialoguePanel.SetActive(false);
        HideOptions();
        dialogueState = 0;

        if (playerInRange)
            interactButtonObject.SetActive(true);
        else
            interactButtonObject.SetActive(false);
    }
}