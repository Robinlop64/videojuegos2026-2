using UnityEngine;

public class Player : Personaje
{
    public bool enSuelo = false;
    public GameObject prefabProyectil;
    public Transform puntoDisparo;
    public AudioClip sonidoDisparo;

    private ControlPlayer controlPlayer;
    private Animator anim;
    private AudioSource audioSrc;

    protected override void Awake()
    {
        base.Awake();
        controlPlayer = GetComponent<ControlPlayer>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Disparar();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enSuelo = true;
        }

    
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enSuelo = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enSuelo = false;
        }
    }

    void Disparar()
    {
        if (prefabProyectil == null || puntoDisparo == null)
        {
            Debug.LogWarning("Prefab proyectil o punto de disparo no asignados");
            return;
        }

        GameObject p = Instantiate(prefabProyectil, puntoDisparo.position, Quaternion.identity);
        Vector2 dir = sr.flipX ? Vector2.left : Vector2.right;
        float velocidadJugador = controlPlayer.velocidadActual;
        p.GetComponent<Proyectil>().Inicializar(dir, velocidadJugador);

        // Animación de disparo (descomentar si existe el trigger "Disparando" en el Animator)
        // if (anim != null)
        //     anim.SetTrigger("Disparando");

        if (audioSrc != null && sonidoDisparo != null)
            audioSrc.PlayOneShot(sonidoDisparo);
    }
}