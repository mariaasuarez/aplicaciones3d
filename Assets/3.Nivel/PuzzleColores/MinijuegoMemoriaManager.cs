using System.Collections; // Necesario para las Corrutinas
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MinijuegoMemoriaManager : MonoBehaviour
{
    [Header("Las 3 Imágenes Ganadoras")]
    [Tooltip("Imagenes correcta")]
    public Texture2D[] imagenesCorrectas = new Texture2D[3];

    [Header("Imágenes de Relleno")]
    [Tooltip("Imágenes incorrectas")]
    public Texture2D[] imagenesDeRelleno;

    [Header("Conexiones de la Escena")]
    [Tooltip("Los 9 botones físicos en el atril")]
    public BotonMemoriaVR[] botonesAtril;
    [Tooltip("El objeto que girara")]
    public GameObject tapaAtril;
    [Tooltip("La recompensa física que aparecerá")]
    public GameObject recompensaObjeto;

    [Header("Configuración Sonora")]
    public AudioSource audioSource;
    public AudioClip sonidoAcierto, sonidoError, sonidoVictoria;

    [Header("Efectos Mágicos")]
    [Tooltip("Tiempo en segundos que tarda en aparecer el objeto")]
    public float duracionAparicion = 1.0f;
    [Tooltip("Arrastra aquí un prefab de partículas para que explote al ganar")]
    public GameObject particulasMagicasPrefab;

    private List<Texture2D> texturasSeleccionadas = new List<Texture2D>();
    private bool juegoTerminado = false;
    private Vector3 escalaOriginalRecompensa;

    void Start()
    {
        if (recompensaObjeto != null)
        {
            escalaOriginalRecompensa = recompensaObjeto.transform.localScale;
            recompensaObjeto.SetActive(false);
        }

        IniciarNuevoJuego();
    }

    public void IniciarNuevoJuego()
    {
        juegoTerminado = false;
        texturasSeleccionadas.Clear();

        foreach (var boton in botonesAtril)
        {
            boton.ResetearBoton();
        }

        List<Texture2D> bolsaDeImagenes = new List<Texture2D>();
        bolsaDeImagenes.AddRange(imagenesCorrectas);

        List<Texture2D> rellenoMezclado = imagenesDeRelleno.OrderBy(x => UnityEngine.Random.value).ToList();

        for (int i = 0; i < 6; i++)
        {
            bolsaDeImagenes.Add(rellenoMezclado[i]);
        }

        bolsaDeImagenes = bolsaDeImagenes.OrderBy(x => UnityEngine.Random.value).ToList();

        for (int i = 0; i < botonesAtril.Length; i++)
        {
            if (botonesAtril[i] != null)
            {
                botonesAtril[i].AsignarImagen(bolsaDeImagenes[i]);
            }
        }
    }

    public bool IntentarSeleccionarBoton(Texture2D imagenPulsada)
    {
        if (juegoTerminado || texturasSeleccionadas.Count >= 3) return false;

        texturasSeleccionadas.Add(imagenPulsada);

        if (texturasSeleccionadas.Count == 3)
        {
            Invoke("VerificarResultado", 0.5f);
        }
        return true;
    }

    public void EliminarSeleccion(Texture2D imagenPulsada)
    {
        if (juegoTerminado) return;
        texturasSeleccionadas.Remove(imagenPulsada);
    }

    void VerificarResultado()
    {
        bool victoria = true;

        foreach (Texture2D seleccion in texturasSeleccionadas)
        {
            if (!imagenesCorrectas.Contains(seleccion))
            {
                victoria = false;
                break;
            }
        }

        if (victoria)
        {
            DeclararVictoria();
        }
        else
        {
            texturasSeleccionadas.Clear();
            foreach (var boton in botonesAtril) boton.ResetearBoton();
            if (audioSource && sonidoError) audioSource.PlayOneShot(sonidoError);
            UnityEngine.Debug.Log("Patrón incorrecto. Reiniciando atril.");
        }
    }

    void DeclararVictoria()
    {
        juegoTerminado = true;
        if (audioSource && sonidoVictoria) audioSource.PlayOneShot(sonidoVictoria);

        if (tapaAtril != null) tapaAtril.transform.Rotate(-90, 0, 0);

        if (recompensaObjeto != null)
        {
            StartCoroutine(AparecerObjetoMagicamente());
        }
    }

    private IEnumerator AparecerObjetoMagicamente()
    {
        if (particulasMagicasPrefab != null)
        {
            Instantiate(particulasMagicasPrefab, recompensaObjeto.transform.position, Quaternion.identity);
        }

        recompensaObjeto.transform.localScale = Vector3.zero;
        recompensaObjeto.SetActive(true);

        float tiempoPasado = 0f;
        while (tiempoPasado < duracionAparicion)
        {
            tiempoPasado += Time.deltaTime;
            float porcentaje = tiempoPasado / duracionAparicion;

            float crecimientoSuave = Mathf.SmoothStep(0f, 1f, porcentaje);

            recompensaObjeto.transform.localScale = Vector3.Lerp(Vector3.zero, escalaOriginalRecompensa, crecimientoSuave);

            yield return null; 
        }
        recompensaObjeto.transform.localScale = escalaOriginalRecompensa;
    }
}