using UnityEngine;

public class EnemyHearing : MonoBehaviour
{
    public float multiplicadorOido = 1f;

    private EnemyAwareness conciencia;

    void Awake()
    {
        conciencia = GetComponent<EnemyAwareness>();
    }

    void OnEnable()
    {
        NoiseSystem.OnNoise += AlOirRuido;
    }

    void OnDisable()
    {
        NoiseSystem.OnNoise -= AlOirRuido;
    }

    void AlOirRuido(Vector3 posicion, float radio)
    {
        if (Vector3.Distance(transform.position, posicion) <= radio * multiplicadorOido)
            conciencia.OirRuido(posicion);
    }
}
