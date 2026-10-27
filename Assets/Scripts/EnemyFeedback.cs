using UnityEngine;

public class EnemyFeedback : MonoBehaviour
{
    public float alturaIndicador = 2.6f;
    public AudioClip sonidoSospecha;
    public AudioClip sonidoAlerta;
    public AudioClip sonidoDetectado;

    private EnemyAwareness conciencia;
    private Renderer cuerpo;
    private AudioSource audioSource;

    void Awake()
    {
        conciencia = GetComponent<EnemyAwareness>();
        cuerpo = GetComponent<Renderer>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.maxDistance = 30f;

        if (sonidoSospecha == null) sonidoSospecha = ToneGenerator.Crear(440f, 0.15f);
        if (sonidoAlerta == null) sonidoAlerta = ToneGenerator.Crear(660f, 0.3f);
        if (sonidoDetectado == null) sonidoDetectado = ToneGenerator.Crear(220f, 0.6f);
    }

    void OnEnable()
    {
        conciencia.OnNivelCambio += AlCambiarNivel;
    }

    void OnDisable()
    {
        conciencia.OnNivelCambio -= AlCambiarNivel;
    }

    void Update()
    {
        if (cuerpo == null) return;

        Color objetivo = conciencia.Nivel.ObtenerColor();
        cuerpo.material.color = Color.Lerp(cuerpo.material.color, objetivo, 6f * Time.deltaTime);
    }

    void AlCambiarNivel(AlertLevel anterior, AlertLevel nuevo)
    {
        if (nuevo <= anterior) return;

        switch (nuevo)
        {
            case AlertLevel.Suspicious:
                audioSource.PlayOneShot(sonidoSospecha);
                break;
            case AlertLevel.Alert:
                audioSource.PlayOneShot(sonidoAlerta);
                break;
            case AlertLevel.Detected:
                audioSource.PlayOneShot(sonidoDetectado);
                break;
        }
    }

    void OnGUI()
    {
        if (conciencia.Conciencia < 0.02f) return;

        Camera camara = Camera.main;
        if (camara == null) return;

        Vector3 pantalla = camara.WorldToScreenPoint(transform.position + Vector3.up * alturaIndicador);
        if (pantalla.z <= 0f) return;

        float x = pantalla.x;
        float y = Screen.height - pantalla.y;
        AlertLevel nivel = conciencia.Nivel;
        Color color = nivel.ObtenerColor();

        GuiUtil.Texto(new Rect(x - 40f, y - 40f, 80f, 40f), nivel.ObtenerSimbolo(), 36, color);
        GuiUtil.Caja(new Rect(x - 30f, y, 60f, 6f), new Color(0f, 0f, 0f, 0.6f));
        GuiUtil.Caja(new Rect(x - 30f, y, 60f * conciencia.Conciencia, 6f), color);
    }
}
