using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool JuegoTerminado { get; private set; }
    public bool Gano { get; private set; }

    public AudioClip sonidoVictoria;
    public AudioClip sonidoDerrota;

    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (sonidoVictoria == null) sonidoVictoria = ToneGenerator.Crear(880f, 0.5f);
        if (sonidoDerrota == null) sonidoDerrota = ToneGenerator.Crear(140f, 0.9f);
    }

    void Update()
    {
        if (JuegoTerminado && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Ganar()
    {
        Terminar(true);
    }

    public void Perder()
    {
        Terminar(false);
    }

    void Terminar(bool victoria)
    {
        if (JuegoTerminado) return;

        JuegoTerminado = true;
        Gano = victoria;
        audioSource.PlayOneShot(victoria ? sonidoVictoria : sonidoDerrota);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnGUI()
    {
        if (!JuegoTerminado) return;

        GuiUtil.Caja(new Rect(0f, 0f, Screen.width, Screen.height), new Color(0f, 0f, 0f, 0.6f));

        string titulo = Gano ? "¡Escapaste!" : "¡Te atraparon!";
        Color color = Gano ? Color.green : Color.red;

        GuiUtil.Texto(new Rect(0f, Screen.height / 2f - 60f, Screen.width, 80f), titulo, 64, color);
        GuiUtil.Texto(new Rect(0f, Screen.height / 2f + 20f, Screen.width, 40f), "Presioná R para reiniciar", 26, Color.white);
    }
}
