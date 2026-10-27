using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float velocidadGiro = 10f;
    public float fuerzaSalto = 7f;
    public float distanciaSuelo = 1.1f;

    [Header("Dash")]
    public float velocidadDash = 20f;
    public float duracionDash = 0.15f;
    public float cooldownDash = 1f;

    private Rigidbody rb;
    private Transform camara;
    private Vector3 direccion;
    private Vector3 direccionDash;
    private bool saltoPedido = false;
    private bool haciendoDash = false;
    private float tiempoRestanteDash = 0f;
    private float proximoDash = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camara = Camera.main.transform;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 adelante = camara.forward;
        adelante.y = 0f;
        adelante.Normalize();

        Vector3 derecha = camara.right;
        derecha.y = 0f;
        derecha.Normalize();

        direccion = (adelante * z + derecha * x).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
            saltoPedido = true;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !haciendoDash && Time.time >= proximoDash)
        {
            haciendoDash = true;
            tiempoRestanteDash = duracionDash;
            proximoDash = Time.time + cooldownDash;
            direccionDash = direccion != Vector3.zero ? direccion : transform.forward;
        }
    }

    void FixedUpdate()
    {
        if (haciendoDash)
        {
            rb.linearVelocity = new Vector3(direccionDash.x * velocidadDash, 0f, direccionDash.z * velocidadDash);
            tiempoRestanteDash -= Time.fixedDeltaTime;
            saltoPedido = false;

            if (tiempoRestanteDash <= 0f)
                haciendoDash = false;

            return;
        }

        float velocidadVertical = rb.linearVelocity.y;
        rb.linearVelocity = new Vector3(direccion.x * velocidad, velocidadVertical, direccion.z * velocidad);

        if (direccion != Vector3.zero)
        {
            Quaternion objetivo = Quaternion.LookRotation(direccion);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, objetivo, velocidadGiro * Time.fixedDeltaTime));
        }

        if (saltoPedido && EstaEnSuelo())
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);

        saltoPedido = false;
    }

    bool EstaEnSuelo()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanciaSuelo);
    }
}