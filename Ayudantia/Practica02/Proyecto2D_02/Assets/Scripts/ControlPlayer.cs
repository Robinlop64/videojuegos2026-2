using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    public float velocidadActual = 0f;
    public float velocidadMax = 5f;
    public float aceleracion = 10f;

    public float velocidadVertical = 0f;
    public float gravedad = -20f;

    public float tiempoMaxSalto = 0.2f;
    private float tiempoSaltoActual = 0f;

    private Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        // Movimiento horizontal
        float h = Input.GetAxis("Horizontal");
        velocidadActual += h * aceleracion * delta;
        velocidadActual = Mathf.Clamp(velocidadActual, -velocidadMax, velocidadMax);
        transform.position += new Vector3(velocidadActual * delta, 0f, 0f);

        // Inicio del salto
        if (Input.GetAxis("Jump") > 0f && player.enSuelo)
        {
            velocidadVertical = 10f;
            player.enSuelo = false;
            tiempoSaltoActual = 0f;
        }

        // Salto sostenido
        if (!player.enSuelo && Input.GetAxis("Jump") > 0f && tiempoSaltoActual < tiempoMaxSalto)
        {
            velocidadVertical += 20f * delta;
            tiempoSaltoActual += delta;
        }

        // Si sueltas el botón
        if (Input.GetAxis("Jump") == 0f)
            tiempoSaltoActual = tiempoMaxSalto;

        // Gravedad / movimiento vertical
        if (player.enSuelo)
        {
            velocidadVertical = 0f;
        }
        else
        {
            velocidadVertical += gravedad * delta;
            transform.position += new Vector3(0f, velocidadVertical * delta, 0f);
        }
    }
}
