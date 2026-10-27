using UnityEngine;

public static class GuiUtil
{
    public static void Caja(Rect rect, Color color)
    {
        Color anterior = GUI.color;
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = anterior;
    }

    public static void Texto(Rect rect, string texto, int tamano, Color color)
    {
        GUIStyle estilo = new GUIStyle(GUI.skin.label);
        estilo.fontSize = tamano;
        estilo.fontStyle = FontStyle.Bold;
        estilo.alignment = TextAnchor.MiddleCenter;

        estilo.normal.textColor = Color.black;
        GUI.Label(new Rect(rect.x + 2f, rect.y + 2f, rect.width, rect.height), texto, estilo);

        estilo.normal.textColor = color;
        GUI.Label(rect, texto, estilo);
    }
}
