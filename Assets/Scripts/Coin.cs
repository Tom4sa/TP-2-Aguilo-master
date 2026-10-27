using UnityEngine;

public class Coin : MonoBehaviour
{
    public float velocidadGiro = 120f;
    public float amplitudFlote = 0.15f;
    public float velocidadFlote = 2f;
    public int valor = 1;
    public AudioClip sonido;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;

        if (sonido == null)
            sonido = ToneGenerator.Crear(1040f, 0.12f, 0.3f);
    }

    void Update()
    {
        transform.Rotate(0f, velocidadGiro * Time.deltaTime, 0f, Space.World);
        float y = Mathf.Sin(Time.time * velocidadFlote) * amplitudFlote;
        transform.position = posicionInicial + new Vector3(0f, y, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        CoinCounter contador = other.GetComponentInParent<CoinCounter>();
        if (contador == null) return;

        contador.Sumar(valor);
        AudioSource.PlayClipAtPoint(sonido, transform.position);
        Destroy(gameObject);
    }
}