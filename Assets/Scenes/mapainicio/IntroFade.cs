using UnityEngine;
using System.Collections;

public class IntroFade : MonoBehaviour
{
    public float duracionFade = 2.0f; // Cuánto tarda en aparecer la escena
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        // Empezamos totalmente en negro
        SetAlpha(1f);
    }

    void Start()
    {
        // Iniciamos la desaparición del negro
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float tiempo = 0;
        Material mat = meshRenderer.material;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
            SetAlpha(alpha);
            yield return null;
        }

        // Al terminar, desactivamos el cubo para ahorrar recursos
        gameObject.SetActive(false);
    }

    // Cambia la función SetAlpha por esta si usas URP
    void SetAlpha(float alpha)
    {
        // URP usa "_BaseColor" en lugar de ".color"
        Color color = meshRenderer.material.GetColor("_BaseColor");
        color.a = alpha;
        meshRenderer.material.SetColor("_BaseColor", color);
    }
}