using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public int Monedas { get; private set; }
    public int Total { get; private set; }

    void Start()
    {
        foreach (Coin moneda in FindObjectsByType<Coin>(FindObjectsSortMode.None))
            Total += moneda.valor;
    }

    public void Sumar(int cantidad)
    {
        Monedas += cantidad;
    }

    void OnGUI()
    {
        GUIStyle estilo = new GUIStyle(GUI.skin.label);
        estilo.fontSize = 28;
        estilo.fontStyle = FontStyle.Bold;
        estilo.normal.textColor = Color.yellow;
        GUI.Label(new Rect(20f, 20f, 400f, 44f), "Monedas: " + Monedas + " / " + Total, estilo);
    }
}