using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    [Header("Movimiento horizontal")]
    public float velocidadActual = 0f;
    public float velocidadMax = 5f;
    public float aceleracion = 10f;
    public float desaceleracion = 8f; 

    [Header("Salto / gravedad")]
    public float fuerzaSalto = 10f;
    public float velocidadVertical = 0f;
    public float gravedad = -20f;
    public float gravedadCaida = -30f; 

    [Header("Salto sostenido (opcional)")]
    public float tiempoMaxSalto = 0.2f;
    private float tiempoSaltoActual = 0f;

    [Header("Coyote / Buffer")]
    public float tiempoCoyote = 0.1f;       
    public float tiempoBufferSalto = 0.1f;  
    private float coyoteTimer = 0f;
    private float bufferTimer = 0f;

    [Header("Estados para animaciones")]
    public bool estaCaminando; 
    public bool estaSaltando;
    public bool estaCayendo;

    private Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        // Input
        float h = Input.GetAxis("Horizontal");
        bool jumpHeld = Input.GetAxis("Jump") > 0f;

        // Aceleración horizontal 
        velocidadActual += h * aceleracion * delta;
        velocidadActual = Mathf.Clamp(velocidadActual, -velocidadMax, velocidadMax);

        // Desaceleración automática cuando no hay input 
        if (h == 0f)
        {
            if (velocidadActual > 0f) velocidadActual -= desaceleracion * delta;
            else if (velocidadActual < 0f) velocidadActual += desaceleracion * delta;

            if (Mathf.Abs(velocidadActual) < 0.1f) velocidadActual = 0f;
        }

        // Coyote time 
        if (player.enSuelo) coyoteTimer = tiempoCoyote;
        else coyoteTimer -= delta;

        // Jump buffering 
        if (jumpHeld) bufferTimer = tiempoBufferSalto;
        else bufferTimer -= delta;

        // Ejecutar salto si hay buffer + coyote 
        if (bufferTimer > 0f && coyoteTimer > 0f)
        {
            velocidadVertical = fuerzaSalto;
            player.enSuelo = false;

            bufferTimer = 0f;
            coyoteTimer = 0f;

            // reinicia “salto sostenido”
            tiempoSaltoActual = 0f;
        }

        // Salto sostenido (mantener botón un ratito)
        if (!player.enSuelo && jumpHeld && tiempoSaltoActual < tiempoMaxSalto && velocidadVertical > 0f)
        {
            velocidadVertical += 20f * delta;
            tiempoSaltoActual += delta;
        }
        if (!jumpHeld) tiempoSaltoActual = tiempoMaxSalto;

        // Gravedad mejorada (caída más rápida) 
        if (player.enSuelo && velocidadVertical <= 0f)
        {
            velocidadVertical = 0f;
        }
        else
        {
            if (velocidadVertical < 0f) velocidadVertical += gravedadCaida * delta;
            else velocidadVertical += gravedad * delta;
        }

        // Movimiento final (X + Y juntos) 
        transform.position += new Vector3(
            velocidadActual * delta,
            velocidadVertical * delta,
            0f
        );

        // Estados para animaciones
        estaCaminando = Mathf.Abs(velocidadActual) > 0.1f;
        estaSaltando = velocidadVertical > 0.1f;
        estaCayendo = velocidadVertical < -0.1f;
    }
}