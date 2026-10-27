using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform objetivo;
    public float distancia = 8f;
    public float distanciaMin = 3f;
    public float distanciaMax = 15f;
    public float altura = 1.5f;
    public float sensibilidad = 2f;
    public float velocidadZoom = 3f;

    public float radioColision = 0.3f;
    public float margen = 0.2f;
    public float distanciaMinimaColision = 0.5f;
    public float velocidadRecuperacion = 8f;

    private float yaw;
    private float pitch = 15f;
    private float distanciaActual;
    private RaycastHit[] impactos = new RaycastHit[16];

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yaw = transform.eulerAngles.y;
        distanciaActual = distancia;
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * sensibilidad;
        pitch -= Input.GetAxis("Mouse Y") * sensibilidad;
        pitch = Mathf.Clamp(pitch, -20f, 60f);

        distancia -= Input.mouseScrollDelta.y * velocidadZoom;
        distancia = Mathf.Clamp(distancia, distanciaMin, distanciaMax);

        Quaternion rotacion = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 punto = objetivo.position + Vector3.up * altura;
        Vector3 haciaAtras = rotacion * Vector3.back;

        float permitida = CalcularDistanciaLibre(punto, haciaAtras);

        if (permitida < distanciaActual)
            distanciaActual = permitida;
        else
            distanciaActual = Mathf.Lerp(distanciaActual, permitida, velocidadRecuperacion * Time.deltaTime);

        transform.position = punto + haciaAtras * distanciaActual;
        transform.rotation = rotacion;
    }

    float CalcularDistanciaLibre(Vector3 origen, Vector3 direccion)
    {
        float libre = distancia;

        int cantidad = Physics.SphereCastNonAlloc(origen, radioColision, direccion, impactos, distancia, ~0, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < cantidad; i++)
        {
            if (impactos[i].transform.IsChildOf(objetivo)) continue;

            float d = impactos[i].distance - margen;
            if (d < libre)
                libre = d;
        }

        return Mathf.Max(libre, distanciaMinimaColision);
    }
}