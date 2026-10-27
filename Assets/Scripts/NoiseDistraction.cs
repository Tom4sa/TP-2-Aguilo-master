using UnityEngine;

public class NoiseDistraction : MonoBehaviour
{
    public float distanciaInteraccion = 2.5f;
    public float radioRuido = 15f;
    public float duracion = 3f;
    public float intervaloRuido = 0.5f;
    public int usosMaximos = 1;
    public Color colorActivo = Color.cyan;
    public AudioClip sonido;

    private Transform jugador;
    private Renderer rend;
    private AudioSource audioSource;
    private Color colorInicial;
    private int usos;
    private bool activo;
    private float tiempoRestante;
    private float tiempoProximoRuido;

    void Start()
    {
        PlayerMovement p = FindFirstObjectByType<PlayerMovement>();
        if (p != null)
            jugador = p.transform;

        rend = GetComponent<Renderer>();
        if (rend != null)
            colorInicial = rend.material.color;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.spatialBlend = 1f;
        audioSource.clip = sonido != null ? sonido : ToneGenerator.Crear(520f, 0.4f, 0.3f);
    }

    void Update()
    {
        if (activo)
        {
            tiempoRestante -= Time.deltaTime;
            tiempoProximoRuido -= Time.deltaTime;

            if (tiempoProximoRuido <= 0f)
            {
                NoiseSystem.Emit(transform.position, radioRuido);
                tiempoProximoRuido = intervaloRuido;
            }

            if (tiempoRestante <= 0f)
                Apagar();

            return;
        }

        if (usos < usosMaximos && JugadorCerca() && Input.GetKeyDown(KeyCode.E))
            Activar();
    }

    void Activar()
    {
        usos++;
        activo = true;
        tiempoRestante = duracion;
        tiempoProximoRuido = 0f;

        if (rend != null)
            rend.material.color = colorActivo;

        audioSource.Play();
    }

    void Apagar()
    {
        activo = false;

        if (rend != null)
            rend.material.color = colorInicial;

        audioSource.Stop();
    }

    bool JugadorCerca()
    {
        return jugador != null && Vector3.Distance(transform.position, jugador.position) <= distanciaInteraccion;
    }

    void OnGUI()
    {
        if (activo || usos >= usosMaximos || !JugadorCerca()) return;

        GuiUtil.Texto(new Rect(Screen.width / 2f - 200f, Screen.height - 120f, 400f, 36f), "[E] Activar distracción", 22, Color.white);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radioRuido);
    }
}
