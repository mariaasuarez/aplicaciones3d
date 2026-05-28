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

    [Header("Botón continuar visual")]
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
    public InputActionReference interactAction;
    public InputActionReference continueAction;
    public InputActionReference option1Action;
    public InputActionReference option2Action;
    public InputActionReference option3Action;

    private bool playerInRange = false;
    private int playerCollidersInside = 0;
    private int dialogueState = 0;
    private int interactionCount = 0;

    private void OnEnable()
    {
        ActivarAccion(interactAction, OnInteractVR);
        ActivarAccion(continueAction, OnContinueVR);
        ActivarAccion(option1Action, OnOption1VR);
        ActivarAccion(option2Action, OnOption2VR);
        ActivarAccion(option3Action, OnOption3VR);
    }

    private void OnDisable()
    {
        DesactivarAccion(interactAction, OnInteractVR);
        DesactivarAccion(continueAction, OnContinueVR);
        DesactivarAccion(option1Action, OnOption1VR);
        DesactivarAccion(option2Action, OnOption2VR);
        DesactivarAccion(option3Action, OnOption3VR);
    }

    private void Start()
    {
        canvasNPC.SetActive(false);
        interactButtonObject.SetActive(false);
        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        HideOptions();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.transform.root.CompareTag("Player")) return;

        playerCollidersInside++;
        playerInRange = true;

        if (dialogueState == 0)
            interactButtonObject.SetActive(true);

        Debug.Log("Jugador cerca del NPC");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") && !other.transform.root.CompareTag("Player")) return;

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
        if (!playerInRange) return;

        interactionCount++;

        canvasNPC.SetActive(true);
        interactButtonObject.SetActive(false);
        dialoguePanel.SetActive(true);
        HideOptions();

        if (interactionCount < 3)
        {
            dialogueText.text = initialDialogue;
            continueButton.gameObject.SetActive(true);
            dialogueState = 1;

            Debug.Log("Diálogo normal iniciado. Interacción: " + interactionCount);
        }
        else
        {
            dialogueText.text = finalDialogue;
            continueButton.gameObject.SetActive(true);
            dialogueState = 4;

            Debug.Log("Tercera interacción: solo diálogo final");
        }
    }

    public void ContinueDialogue()
    {
        Debug.Log("BOTÓN CONTINUAR OPRIMIDO");

        if (!playerInRange) return;
        if (!canvasNPC.activeSelf) return;

        if (dialogueState == 1)
        {
            dialogueText.text = "";
            continueButton.gameObject.SetActive(false);
            ShowOptions();
            dialogueState = 2;

            Debug.Log("Aparecieron las opciones");
        }
        else if (dialogueState == 3)
        {
            dialogueText.text = finalDialogue;
            continueButton.gameObject.SetActive(true);
            dialogueState = 4;

            Debug.Log("Mostrando diálogo final");
        }
        else if (dialogueState == 4)
        {
            CloseDialogue();

            Debug.Log("Diálogo cerrado");
        }
    }

    public void ChooseOption(int option)
    {
        if (!playerInRange) return;
        if (!canvasNPC.activeSelf) return;
        if (dialogueState != 2) return;

        HideOptions();

        if (option == 1)
        {
            dialogueText.text = response1;
            Debug.Log("OPCIÓN 1 OPRIMIDA");
        }
        else if (option == 2)
        {
            dialogueText.text = response2;
            Debug.Log("OPCIÓN 2 OPRIMIDA");
        }
        else if (option == 3)
        {
            dialogueText.text = response3;
            Debug.Log("OPCIÓN 3 OPRIMIDA");
        }

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

    private void CloseDialogue()
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

    private void OnInteractVR(InputAction.CallbackContext context)
    {
        Debug.Log("BOTÓN INTERACT OPRIMIDO");
        Interact();
    }

    private void OnContinueVR(InputAction.CallbackContext context)
    {
        Debug.Log("INPUT CONTINUE DETECTADO");
        ContinueDialogue();
    }

    private void OnOption1VR(InputAction.CallbackContext context)
    {
        ChooseOption(1);
    }

    private void OnOption2VR(InputAction.CallbackContext context)
    {
        ChooseOption(2);
    }

    private void OnOption3VR(InputAction.CallbackContext context)
    {
        ChooseOption(3);
    }

    private void ActivarAccion(InputActionReference accion, System.Action<InputAction.CallbackContext> metodo)
    {
        if (accion == null) return;

        accion.action.Enable();
        accion.action.performed += metodo;
    }

    private void DesactivarAccion(InputActionReference accion, System.Action<InputAction.CallbackContext> metodo)
    {
        if (accion == null) return;

        accion.action.performed -= metodo;
    }
}