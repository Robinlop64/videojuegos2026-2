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

    [Header("Audio")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoPaso;
    private AudioSource audioSrc;

    private Player player;
    private Animator anim;

    void Awake()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        float h = Input.GetAxis("Horizontal");
        bool jumpHeld = Input.GetAxis("Jump") > 0f;

        velocidadActual += h * aceleracion * delta;
        velocidadActual = Mathf.Clamp(velocidadActual, -velocidadMax, velocidadMax);

        if (h == 0f)
        {
            if (velocidadActual > 0f) velocidadActual -= desaceleracion * delta;
            else if (velocidadActual < 0f) velocidadActual += desaceleracion * delta;

            if (Mathf.Abs(velocidadActual) < 0.1f) velocidadActual = 0f;
        }

        if (player.enSuelo) coyoteTimer = tiempoCoyote;
        else coyoteTimer -= delta;

        if (jumpHeld) bufferTimer = tiempoBufferSalto;
        else bufferTimer -= delta;

        if (bufferTimer > 0f && coyoteTimer > 0f)
        {
            velocidadVertical = fuerzaSalto;
            player.enSuelo = false;

            bufferTimer = 0f;
            coyoteTimer = 0f;
            tiempoSaltoActual = 0f;

            if (audioSrc != null && sonidoSalto != null)
                audioSrc.PlayOneShot(sonidoSalto);
        }

        if (!player.enSuelo && jumpHeld && tiempoSaltoActual < tiempoMaxSalto && velocidadVertical > 0f)
        {
            velocidadVertical += 20f * delta;
            tiempoSaltoActual += delta;
        }

        if (!jumpHeld) tiempoSaltoActual = tiempoMaxSalto;

        if (player.enSuelo && velocidadVertical <= 0f)
        {
            velocidadVertical = 0f;
        }
        else
        {
            if (velocidadVertical < 0f) velocidadVertical += gravedadCaida * delta;
            else velocidadVertical += gravedad * delta;
        }

        transform.position += new Vector3(
            velocidadActual * delta,
            velocidadVertical * delta,
            0f
        );

        ActualizarAnimaciones();

        if (estaCaminando && player.enSuelo)
        {
            if (!audioSrc.isPlaying && sonidoPaso != null)
                audioSrc.PlayOneShot(sonidoPaso);
        }
    }

    void ActualizarAnimaciones()
    {
        bool enSuelo = player.enSuelo;

        estaCaminando = enSuelo && Mathf.Abs(velocidadActual) > 0.1f;
        estaSaltando = !enSuelo && velocidadVertical > 0.1f;
        estaCayendo = !enSuelo && velocidadVertical < -0.1f;

        // Cambiar dirección del sprite según la velocidad
        if (velocidadActual > 0.1f)
        {
            player.sr.flipX = false; // Hacia la derecha
        }
        else if (velocidadActual < -0.1f)
        {
            player.sr.flipX = true; // Hacia la izquierda
        }

        if (anim != null)
        {
            anim.SetBool("Caminando", estaCaminando);
            anim.SetBool("Saltando", estaSaltando);
            anim.SetBool("Cayendo", estaCayendo);
        }
    }
}