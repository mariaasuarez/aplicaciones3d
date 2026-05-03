using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// VR Tutorial Book - Versión con imágenes por página
/// Cada página es un sprite/imagen diseñado externamente
/// 
/// TECLADO (pruebas sin gafas):
///   T          = Abrir / Cerrar libro
///   → o D      = Página siguiente  
///   ← o A      = Página anterior
///   Escape     = Cerrar libro
/// 
/// VR (Quest):
///   Botón X    = Abrir / Cerrar
///   Grip Der   = Página siguiente
///   Grip Izq   = Página anterior
/// </summary>
public class TutorialBookController : MonoBehaviour
{
    // ══════════════════════════════════════════
    // SINGLETON – cualquier botón en cualquier escena puede llamarlo
    // ══════════════════════════════════════════
    public static TutorialBookController Instance { get; private set; }

    // ─────────────────────────────────────────
    [Header("=== OBJETO DEL LIBRO ===")]
    [Tooltip("Arrastra aquí el BookCanvas desde la jerarquía")]
    public GameObject bookObject;

    [Tooltip("Se rellena automáticamente en cada escena. Puedes dejarlo vacío.")]
    public Transform vrCamera;

    // ─────────────────────────────────────────
    [Header("=== IMÁGENES DE LAS PÁGINAS ===")]
    [Tooltip("Componente Image de la página IZQUIERDA del libro")]
    public Image leftPageImage;

    [Tooltip("Componente Image de la página DERECHA del libro")]
    public Image rightPageImage;

    [Space(5)]
    [Tooltip("Los 6 sprites de la página izquierda (en orden)")]
    public Sprite[] leftPageSprites;

    [Tooltip("Los 6 sprites de la página derecha (en orden)")]
    public Sprite[] rightPageSprites;

    // ─────────────────────────────────────────
    [Header("=== BOTONES DE NAVEGACIÓN ===")]
    [Tooltip("GameObject del botón anterior (se oculta en pág 1)")]
    public GameObject prevButton;

    [Tooltip("GameObject del botón siguiente (se oculta en última pág)")]
    public GameObject nextButton;

    [Tooltip("(Opcional) Texto que muestra '1 / 6'")]
    public TextMeshProUGUI pageNumberText;

    // ─────────────────────────────────────────
    [Header("=== INPUT ACTIONS VR ===")]
    [Tooltip("Input Action para abrir/cerrar (botón X izquierdo)")]
    public InputActionReference openCloseAction;

    [Tooltip("Input Action para página siguiente (Grip derecho)")]
    public InputActionReference nextPageAction;

    [Tooltip("Input Action para página anterior (Grip izquierdo)")]
    public InputActionReference prevPageAction;

    // ─────────────────────────────────────────
    [Header("=== POSICIÓN EN EL MUNDO ===")]
    [Tooltip("Qué tan lejos aparece el libro frente al jugador (metros)")]
    public float spawnDistance = 1.2f;

    [Tooltip("Ajuste de altura respecto al jugador")]
    public float heightOffset = -0.1f;

    [Tooltip("Velocidad de la animación de aparición")]
    public float appearSpeed = 8f;

    // ─────────────────────────────────────────
    // Variables internas
    private int currentPage = 0;
    private bool isOpen = false;
    private bool isAnimating = false;
    private float inputCooldown = 0f;
    private const float INPUT_DELAY = 0.35f;

    // ══════════════════════════════════════════
    // UNITY EVENTS
    // ══════════════════════════════════════════

    void Awake()
    {
        // Singleton: solo existe una instancia
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // Cerrar libro al inicio
        if (bookObject != null)
        {
            bookObject.SetActive(false);
            bookObject.transform.localScale = Vector3.zero;
        }

        // Buscar cámara inicial
        RefreshCamera();

        // Suscribirse al evento de carga de escena para actualizar la cámara
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Registrar inputs VR
        if (openCloseAction != null)
            openCloseAction.action.performed += _ => ToggleBook();
        if (nextPageAction != null)
            nextPageAction.action.performed += _ => { if (isOpen) GoToNextPage(); };
        if (prevPageAction != null)
            prevPageAction.action.performed += _ => { if (isOpen) GoToPrevPage(); };
    }

    void OnEnable()
    {
        if (openCloseAction != null) openCloseAction.action.Enable();
        if (nextPageAction != null) nextPageAction.action.Enable();
        if (prevPageAction != null) prevPageAction.action.Enable();
    }

    void OnDisable()
    {
        if (openCloseAction != null) openCloseAction.action.performed -= _ => ToggleBook();
        if (nextPageAction != null) nextPageAction.action.performed -= _ => GoToNextPage();
        if (prevPageAction != null) prevPageAction.action.performed -= _ => GoToPrevPage();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        // Reducir cooldown
        if (inputCooldown > 0) inputCooldown -= Time.deltaTime;

        // ── TECLADO PARA PRUEBAS ──────────────────
        if (Input.GetKeyDown(KeyCode.T) && inputCooldown <= 0)
        {
            inputCooldown = INPUT_DELAY;
            ToggleBook();
        }

        if (isOpen && inputCooldown <= 0)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                GoToNextPage();

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                GoToPrevPage();

            if (Input.GetKeyDown(KeyCode.Escape))
                CloseBook();
        }

        // ── LIBRO MIRA AL JUGADOR (suave) ─────────
        // FIX: usamos (cámara - libro) para que la CARA del canvas apunte al jugador
        if (isOpen && bookObject != null && bookObject.activeSelf && vrCamera != null)
        {
            Vector3 dir = bookObject.transform.position - vrCamera.position;
            dir.y = 0;
            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
                bookObject.transform.rotation = Quaternion.Slerp(
                    bookObject.transform.rotation, targetRot, Time.deltaTime * 3f);
            }
        }
    }

    // ══════════════════════════════════════════
    // GESTIÓN DE ESCENAS
    // ══════════════════════════════════════════

    /// <summary>
    /// Se llama automáticamente cada vez que se carga una nueva escena.
    /// Espera un frame para que la escena esté inicializada y busca la cámara.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(RefreshCameraNextFrame());
    }

    private IEnumerator RefreshCameraNextFrame()
    {
        yield return null; // esperar un frame a que la escena esté lista
        RefreshCamera();
    }

    /// <summary>
    /// Busca Camera.main en la escena activa y actualiza vrCamera.
    /// </summary>
    private void RefreshCamera()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            vrCamera = cam.transform;
        }
        else
        {
            Debug.LogWarning("[TutorialBook] No se encontró Camera.main en la escena. " +
                             "Asegúrate de que la cámara tiene el tag 'MainCamera'.");
        }
    }

    // ══════════════════════════════════════════
    // ABRIR / CERRAR
    // ══════════════════════════════════════════

    public void ToggleBook()
    {
        if (isOpen) CloseBook();
        else OpenBook();
    }

    public void OpenBook()
    {
        if (bookObject == null || vrCamera == null)
        {
            RefreshCamera(); // último intento de encontrar la cámara
            if (vrCamera == null) return;
        }

        isOpen = true;
        currentPage = 0;

        // ── Calcular dirección frente a la cámara ──
        Vector3 forward = vrCamera.forward;
        forward.y = 0;
        if (forward.sqrMagnitude < 0.001f) forward = Vector3.forward;
        forward.Normalize();

        // Posicionar el libro frente al jugador
        bookObject.transform.position = vrCamera.position
                                        + forward * spawnDistance
                                        + Vector3.up * heightOffset;

        // FIX PRINCIPAL: LookRotation con el vector HACIA la cámara (-forward)
        // para que la cara visible del canvas quede de frente al jugador.
        bookObject.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);

        bookObject.transform.localScale = Vector3.zero;
        bookObject.SetActive(true);

        UpdatePage();
        StartCoroutine(AnimateScale(Vector3.zero, Vector3.one, true));
    }

    public void CloseBook()
    {
        isOpen = false;
        StartCoroutine(AnimateScale(bookObject.transform.localScale, Vector3.zero, false));
    }

    // ══════════════════════════════════════════
    // NAVEGACIÓN
    // ══════════════════════════════════════════

    public void GoToNextPage()
    {
        if (isAnimating || inputCooldown > 0) return;
        if (currentPage >= TotalPages() - 1) return;

        currentPage++;
        inputCooldown = INPUT_DELAY;
        StartCoroutine(AnimatePageFlip(true));
    }

    public void GoToPrevPage()
    {
        if (isAnimating || inputCooldown > 0) return;
        if (currentPage <= 0) return;

        currentPage--;
        inputCooldown = INPUT_DELAY;
        StartCoroutine(AnimatePageFlip(false));
    }

    private int TotalPages()
    {
        int l = leftPageSprites != null ? leftPageSprites.Length : 0;
        int r = rightPageSprites != null ? rightPageSprites.Length : 0;
        return Mathf.Max(l, r);
    }

    // ══════════════════════════════════════════
    // ACTUALIZAR CONTENIDO
    // ══════════════════════════════════════════

    private void UpdatePage()
    {
        if (leftPageImage != null && leftPageSprites != null
            && currentPage < leftPageSprites.Length)
            leftPageImage.sprite = leftPageSprites[currentPage];

        if (rightPageImage != null && rightPageSprites != null
            && currentPage < rightPageSprites.Length)
            rightPageImage.sprite = rightPageSprites[currentPage];

        if (pageNumberText != null)
            pageNumberText.text = $"{currentPage + 1}  /  {TotalPages()}";

        if (prevButton != null) prevButton.SetActive(currentPage > 0);
        if (nextButton != null) nextButton.SetActive(currentPage < TotalPages() - 1);
    }

    // ══════════════════════════════════════════
    // ANIMACIONES
    // ══════════════════════════════════════════

    private IEnumerator AnimateScale(Vector3 from, Vector3 to, bool opening)
    {
        float t = 0f;
        float duration = 0.25f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float ease = opening ? EaseOutBack(Mathf.Clamp01(t))
                                 : EaseInBack(Mathf.Clamp01(t));
            bookObject.transform.localScale = Vector3.LerpUnclamped(from, to, ease);
            yield return null;
        }

        bookObject.transform.localScale = to;
        if (!opening) bookObject.SetActive(false);
    }

    private IEnumerator AnimatePageFlip(bool goingForward)
    {
        isAnimating = true;

        CanvasGroup cg = bookObject.GetComponent<CanvasGroup>();
        if (cg == null) cg = bookObject.AddComponent<CanvasGroup>();

        float duration = 0.18f;
        float t = 0f;
        float slideDir = goingForward ? -1f : 1f;
        Vector3 originalPos = bookObject.transform.localPosition;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            cg.alpha = Mathf.Lerp(1f, 0.2f, t);
            bookObject.transform.localPosition = originalPos
                + new Vector3(slideDir * 40f * t, 0f, 0f);
            yield return null;
        }

        UpdatePage();

        t = 0f;
        bookObject.transform.localPosition = originalPos
            + new Vector3(-slideDir * 40f, 0f, 0f);

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            cg.alpha = Mathf.Lerp(0.2f, 1f, t);
            bookObject.transform.localPosition = Vector3.Lerp(
                originalPos + new Vector3(-slideDir * 40f, 0f, 0f),
                originalPos, t);
            yield return null;
        }

        cg.alpha = 1f;
        bookObject.transform.localPosition = originalPos;
        isAnimating = false;
    }

    // ══════════════════════════════════════════
    // EASING
    // ══════════════════════════════════════════

    private float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    private float EaseInBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return c3 * t * t * t - c1 * t * t;
    }
}