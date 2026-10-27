using UnityEngine;

public class PlayerStealth : MonoBehaviour
{
    public float visibilidadAgachado = 0.5f;
    public float factorVelocidadAgachado = 0.5f;
    public float escalaAgachado = 0.6f;
    public float radioRuidoDash = 12f;

    public float MultiplicadorZona { get; set; } = 1f;
    public bool EstaAgachado { get; private set; }

    public float Visibilidad
    {
        get { return (EstaAgachado ? visibilidadAgachado : 1f) * MultiplicadorZona; }
    }

    private PlayerMovement movimiento;
    private float velocidadBase;
    private Vector3 escalaBase;

    void Start()
    {
        movimiento = GetComponent<PlayerMovement>();
        if (movimiento != null)
            velocidadBase = movimiento.velocidad;

        escalaBase = transform.localScale;
    }

    void Update()
    {
        bool agachar = Input.GetKey(KeyCode.LeftControl);
        if (agachar != EstaAgachado)
            CambiarPostura(agachar);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            NoiseSystem.Emit(transform.position, radioRuidoDash);
    }

    void CambiarPostura(bool agachar)
    {
        EstaAgachado = agachar;

        float factor = agachar ? escalaAgachado : 1f;
        transform.localScale = new Vector3(escalaBase.x, escalaBase.y * factor, escalaBase.z);

        if (movimiento != null)
            movimiento.velocidad = agachar ? velocidadBase * factorVelocidadAgachado : velocidadBase;

        if (!agachar)
            transform.position += Vector3.up * escalaBase.y * (1f - escalaAgachado);
    }
}
