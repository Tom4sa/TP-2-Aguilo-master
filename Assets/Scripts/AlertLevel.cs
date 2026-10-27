using UnityEngine;

public enum AlertLevel
{
    Calm,
    Suspicious,
    Alert,
    Detected
}

public static class AlertLevelExtensions
{
    public static Color ObtenerColor(this AlertLevel nivel)
    {
        switch (nivel)
        {
            case AlertLevel.Suspicious: return Color.yellow;
            case AlertLevel.Alert: return new Color(1f, 0.5f, 0f);
            case AlertLevel.Detected: return Color.red;
            default: return Color.gray;
        }
    }

    public static string ObtenerNombre(this AlertLevel nivel)
    {
        switch (nivel)
        {
            case AlertLevel.Suspicious: return "Sospechoso";
            case AlertLevel.Alert: return "Alerta";
            case AlertLevel.Detected: return "Detectado";
            default: return "Tranquilo";
        }
    }

    public static string ObtenerSimbolo(this AlertLevel nivel)
    {
        switch (nivel)
        {
            case AlertLevel.Suspicious: return "?";
            case AlertLevel.Alert: return "!";
            case AlertLevel.Detected: return "!!";
            default: return "";
        }
    }
}
