using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public Transform jugador;
    public float distanciaVision = 10f;
    public float anguloVision = 90f;
    public float alturaOjos = 1f;

    private PlayerStealth sigilo;
    private RaycastHit[] impactos = new RaycastHit[16];

    public Vector3 PosicionJugador
    {
        get { return jugador != null ? jugador.position : transform.position; }
    }

    void Start()
    {
        if (jugador == null)
        {
            PlayerMovement p = FindFirstObjectByType<PlayerMovement>();
            if (p != null)
                jugador = p.transform;
        }

        if (jugador != null)
            sigilo = jugador.GetComponent<PlayerStealth>();
    }

    public float NivelVisibilidad()
    {
        if (jugador == null) return 0f;

        Vector3 origen = transform.position + Vector3.up * alturaOjos;
        Vector3 objetivo = jugador.position + Vector3.up * 0.5f;
        Vector3 direccion = objetivo - origen;
        float distancia = direccion.magnitude;

        float alcance = distanciaVision * (sigilo != null ? sigilo.Visibilidad : 1f);
        if (alcance <= 0f || distancia > alcance) return 0f;
        if (Vector3.Angle(transform.forward, direccion) > anguloVision / 2f) return 0f;
        if (HayObstaculo(origen, direccion.normalized, distancia)) return 0f;

        float cercania = 1f - distancia / alcance;
        return Mathf.Lerp(0.3f, 1f, cercania);
    }

    bool HayObstaculo(Vector3 origen, Vector3 direccion, float distancia)
    {
        int cantidad = Physics.RaycastNonAlloc(origen, direccion, impactos, distancia, ~0, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < cantidad; i++)
        {
            Transform t = impactos[i].transform;

            if (t.IsChildOf(jugador)) continue;
            if (t.IsChildOf(transform)) continue;

            return true;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 origen = transform.position + Vector3.up * alturaOjos;
        Vector3 izquierda = Quaternion.Euler(0f, -anguloVision / 2f, 0f) * transform.forward;
        Vector3 derecha = Quaternion.Euler(0f, anguloVision / 2f, 0f) * transform.forward;
        Gizmos.DrawRay(origen, izquierda * distanciaVision);
        Gizmos.DrawRay(origen, derecha * distanciaVision);
    }
}
