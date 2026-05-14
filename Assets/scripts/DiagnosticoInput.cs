using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DiagnosticoInput : MonoBehaviour
{
    public InputActionReference moveAction;
    public TextMeshProUGUI textoDebug;

    void Update()
    {
        if (moveAction != null && moveAction.action != null)
        {
            Vector2 valor = moveAction.action.ReadValue<Vector2>();
            string estado = "Move valor: " + valor.ToString();

            if (textoDebug != null)
                textoDebug.text = estado;

            if (valor.magnitude > 0.1f)
                Debug.Log("MOVIMIENTO: " + valor);
        }
    }
}