using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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

    [Header("Botones VR")]
    public InputActionReference vrInteract;
    public InputActionReference vrContinue;
    public InputActionReference vrOption1;
    public InputActionReference vrOption2;
    public InputActionReference vrOption3;

    private bool playerInRange = false;
    private int dialogueState = 0;
    private int playerCollidersInside = 0;

    void OnEnable()
    {
        if (vrInteract != null) vrInteract.action.performed += OnVRInteract;
        if (vrContinue != null) vrContinue.action.performed += OnVRContinue;
        if (vrOption1 != null) vrOption1.action.performed += OnVROption1;
        if (vrOption2 != null) vrOption2.action.performed += OnVROption2;
        if (vrOption3 != null) vrOption3.action.performed += OnVROption3;
    }

    void OnDisable()
    {
        if (vrInteract != null) vrInteract.action.performed -= OnVRInteract;
        if (vrContinue != null) vrContinue.action.performed -= OnVRContinue;
        if (vrOption1 != null) vrOption1.action.performed -= OnVROption1;
        if (vrOption2 != null) vrOption2.action.performed -= OnVROption2;
        if (vrOption3 != null) vrOption3.action.performed -= OnVROption3;
    }

    void Start()
    {
        canvasNPC.SetActive(false);
        interactButtonObject.SetActive(false);
        dialoguePanel.SetActive(false);
        HideOptions();

        continueButton.gameObject.SetActive(false);

        if (vrInteract != null) vrInteract.action.Enable();
        if (vrContinue != null) vrContinue.action.Enable();
        if (vrOption1 != null) vrOption1.action.Enable();
        if (vrOption2 != null) vrOption2.action.Enable();
        if (vrOption3 != null) vrOption3.action.Enable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerCollidersInside++;
        playerInRange = true;

        if (dialogueState == 0)
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

    void OnVRInteract(InputAction.CallbackContext context)
    {
        if (playerInRange && dialogueState == 0)
        {
            Interact();
        }
    }

    void OnVRContinue(InputAction.CallbackContext context)
    {
        if (continueButton.gameObject.activeSelf)
        {
            ContinueDialogue();
        }
    }

    void OnVROption1(InputAction.CallbackContext context)
    {
        if (optionButton1.activeSelf)
        {
            ChooseOption(1);
        }
    }

    void OnVROption2(InputAction.CallbackContext context)
    {
        if (optionButton2.activeSelf)
        {
            ChooseOption(2);
        }
    }

    void OnVROption3(InputAction.CallbackContext context)
    {
        if (optionButton3.activeSelf)
        {
            ChooseOption(3);
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
        continueButton.gameObject.SetActive(false);
        HideOptions();

        dialogueState = 0;

        if (playerInRange)
            interactButtonObject.SetActive(true);
        else
            interactButtonObject.SetActive(false);
    }
}