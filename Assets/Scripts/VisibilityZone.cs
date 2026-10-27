using UnityEngine;

public class VisibilityZone : MonoBehaviour
{
    public float multiplicador = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        PlayerStealth sigilo = other.GetComponent<PlayerStealth>();
        if (sigilo != null)
            sigilo.MultiplicadorZona = multiplicador;
    }

    void OnTriggerExit(Collider other)
    {
        PlayerStealth sigilo = other.GetComponent<PlayerStealth>();
        if (sigilo != null)
            sigilo.MultiplicadorZona = 1f;
    }
}
