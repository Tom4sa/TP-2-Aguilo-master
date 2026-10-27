using UnityEngine;

public static class ToneGenerator
{
    public static AudioClip Crear(float frecuencia, float duracion, float volumen = 0.4f)
    {
        int frecuenciaMuestreo = 44100;
        int muestras = Mathf.Max(1, (int)(frecuenciaMuestreo * duracion));
        float[] datos = new float[muestras];

        for (int i = 0; i < muestras; i++)
        {
            float t = (float)i / frecuenciaMuestreo;
            float envolvente = 1f - (float)i / muestras;
            datos[i] = Mathf.Sin(2f * Mathf.PI * frecuencia * t) * envolvente * volumen;
        }

        AudioClip clip = AudioClip.Create("Tono", muestras, 1, frecuenciaMuestreo, false);
        clip.SetData(datos, 0);
        return clip;
    }
}
