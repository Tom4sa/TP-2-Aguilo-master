using UnityEngine;

public class StealthHUD : MonoBehaviour
{
    public PlayerStealth jugador;

    void Start()
    {
        if (jugador == null)
            jugador = FindFirstObjectByType<PlayerStealth>();
    }

    void OnGUI()
    {
        AlertLevel nivel = AlertLevel.Calm;
        float conciencia = 0f;

        foreach (EnemyAwareness enemigo in EnemyAwareness.Todos)
        {
            if (enemigo.Nivel > nivel) nivel = enemigo.Nivel;
            if (enemigo.Conciencia > conciencia) conciencia = enemigo.Conciencia;
        }

        float centro = Screen.width / 2f;
        Color color = nivel.ObtenerColor();

        GuiUtil.Texto(new Rect(centro - 150f, 10f, 300f, 36f), "ESTADO: " + nivel.ObtenerNombre().ToUpper(), 24, color);
        GuiUtil.Caja(new Rect(centro - 150f, 48f, 300f, 10f), new Color(0f, 0f, 0f, 0.6f));
        GuiUtil.Caja(new Rect(centro - 150f, 48f, 300f * conciencia, 10f), color);

        if (jugador != null)
        {
            string postura = jugador.EstaAgachado ? "Agachado" : "De pie";
            string zona = "";
            if (jugador.MultiplicadorZona < 1f) zona = " | En sombra";
            else if (jugador.MultiplicadorZona > 1f) zona = " | Expuesto";

            string texto = "Visibilidad: " + Mathf.RoundToInt(jugador.Visibilidad * 100f) + "% (" + postura + zona + ")";
            GuiUtil.Texto(new Rect(centro - 200f, 62f, 400f, 28f), texto, 16, Color.white);
        }

        GuiUtil.Texto(new Rect(centro - 300f, Screen.height - 40f, 600f, 30f), "WASD mover | Ctrl agacharse | Shift dash | E interactuar | Mouse cámara", 16, Color.white);
    }
}
