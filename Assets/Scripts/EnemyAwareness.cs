using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public static readonly List<EnemyAwareness> Todos = new List<EnemyAwareness>();

    public float velocidadDeteccion = 0.7f;
    public float velocidadOlvido = 0.2f;
    public float retrasoOlvido = 4f;
    public float umbralSospecha = 0.3f;
    public float umbralAlerta = 0.65f;

    public event Action<AlertLevel, AlertLevel> OnNivelCambio;

    public float Conciencia { get; private set; }
    public AlertLevel Nivel { get; private set; }
    public Vector3 UltimaPosicionConocida { get; private set; }
    public bool VeAlJugador { get; private set; }

    private EnemyVision vision;
    private float tiempoSinEstimulo;

    void Awake()
    {
        vision = GetComponent<EnemyVision>();
        UltimaPosicionConocida = transform.position;
    }

    void OnEnable()
    {
        Todos.Add(this);
    }

    void OnDisable()
    {
        Todos.Remove(this);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.JuegoTerminado) return;

        float visibilidad = vision != null ? vision.NivelVisibilidad() : 0f;
        VeAlJugador = visibilidad > 0f;

        if (VeAlJugador)
        {
            Conciencia += velocidadDeteccion * visibilidad * Time.deltaTime;
            UltimaPosicionConocida = vision.PosicionJugador;
            tiempoSinEstimulo = 0f;
        }
        else
        {
            tiempoSinEstimulo += Time.deltaTime;
            if (tiempoSinEstimulo > retrasoOlvido)
                Conciencia -= velocidadOlvido * Time.deltaTime;
        }

        Conciencia = Mathf.Clamp01(Conciencia);
        ActualizarNivel();
    }

    public void OirRuido(Vector3 posicion)
    {
        UltimaPosicionConocida = posicion;
        tiempoSinEstimulo = 0f;
        Conciencia = Mathf.Max(Conciencia, umbralAlerta + 0.05f);
    }

    void ActualizarNivel()
    {
        AlertLevel nuevo = AlertLevel.Calm;

        if (Conciencia >= 1f) nuevo = AlertLevel.Detected;
        else if (Conciencia >= umbralAlerta) nuevo = AlertLevel.Alert;
        else if (Conciencia >= umbralSospecha) nuevo = AlertLevel.Suspicious;

        if (nuevo == Nivel) return;

        AlertLevel anterior = Nivel;
        Nivel = nuevo;

        if (OnNivelCambio != null)
            OnNivelCambio(anterior, nuevo);

        if (nuevo == AlertLevel.Detected && GameManager.Instance != null)
            GameManager.Instance.Perder();
    }
}
