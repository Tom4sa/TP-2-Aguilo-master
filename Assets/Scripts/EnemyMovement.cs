using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] puntos;
    public float velocidadPatrulla = 2f;
    public float velocidadAlerta = 4f;
    public float velocidadGiro = 5f;
    public float velocidadEscaneo = 60f;
    public float esperaEnPunto = 1f;

    private EnemyAwareness conciencia;
    private int indice;
    private float tiempoEspera;

    void Awake()
    {
        conciencia = GetComponent<EnemyAwareness>();
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.JuegoTerminado) return;

        switch (conciencia.Nivel)
        {
            case AlertLevel.Calm:
                Patrullar();
                break;
            case AlertLevel.Suspicious:
                Mirar(conciencia.UltimaPosicionConocida);
                break;
            case AlertLevel.Alert:
                Investigar();
                break;
        }
    }

    void Patrullar()
    {
        if (puntos == null || puntos.Length == 0) return;

        if (!IrA(puntos[indice].position, velocidadPatrulla))
        {
            tiempoEspera = 0f;
            return;
        }

        tiempoEspera += Time.deltaTime;
        if (tiempoEspera >= esperaEnPunto)
        {
            tiempoEspera = 0f;
            indice = (indice + 1) % puntos.Length;
        }
    }

    void Investigar()
    {
        if (IrA(conciencia.UltimaPosicionConocida, velocidadAlerta))
            transform.Rotate(0f, velocidadEscaneo * Time.deltaTime, 0f);
    }

    bool IrA(Vector3 destino, float velocidad)
    {
        destino.y = transform.position.y;
        Vector3 direccion = destino - transform.position;

        if (direccion.magnitude < 0.3f) return true;

        Quaternion objetivo = Quaternion.LookRotation(direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, objetivo, velocidadGiro * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
        return false;
    }

    void Mirar(Vector3 punto)
    {
        Vector3 direccion = punto - transform.position;
        direccion.y = 0f;

        if (direccion.sqrMagnitude < 0.01f) return;

        Quaternion objetivo = Quaternion.LookRotation(direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, objetivo, velocidadGiro * Time.deltaTime);
    }
}
