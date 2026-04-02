using UnityEngine;
using UnityEngine.SceneManagement;

public class FinDelJuego : MonoBehaviour
{
    public void Ganar()
    {
        SceneManager.LoadScene("Cinematica2");
    }
}