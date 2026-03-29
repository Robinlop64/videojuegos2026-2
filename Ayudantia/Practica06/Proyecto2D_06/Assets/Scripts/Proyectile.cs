using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidadBase = 10f;
    public float tiempoVida = 2f;
    public int dano = 1;
    private Vector2 direccion;
    private float velocidadExtra = 0f;
    public void Inicializar(Vector2 dir, float velocidadJugador)
    {
        direccion = dir.normalized;
        velocidadExtra = velocidadJugador;
        Destroy(gameObject, tiempoVida);
    }
    void Update()
    {
        float velocidadFinal = velocidadBase + velocidadExtra;
        transform.position += (Vector3)(direccion * velocidadFinal *
        Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Proyectil colisionó con " + col.gameObject.name);
        if (col.CompareTag("Enemigo"))
        {
            col.GetComponent<EnemigoIA>().RecibirDano(dano);
            Debug.Log("Impacto contra " + col.gameObject.name);
            Destroy(gameObject);
        }
        if (col.CompareTag("Pared") || col.CompareTag("Obstaculo"))
        {
            Destroy(gameObject);
        }
    }
}