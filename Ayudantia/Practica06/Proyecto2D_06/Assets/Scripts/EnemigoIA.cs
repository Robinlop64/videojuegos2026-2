using UnityEngine;

public class EnemigoIA : Personaje
{
    public float direccion = -1f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direccion * velocidad, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Pared") || col.gameObject.CompareTag("Obstaculo"))
        {
            direccion *= -1f;
            sr.flipX = !sr.flipX;
            return;
        }

        if (col.gameObject.CompareTag("Player"))
        {
            Player jugador = col.gameObject.GetComponent<Player>();

            if (jugador == null)
                return;

            if (col.contacts.Length > 0 && col.contacts[0].normal.y < 0)
            {
                RecibirDano(1);
            }
            else
            {
                jugador.RecibirDano(1);
            }
        }
    }
}