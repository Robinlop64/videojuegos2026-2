using UnityEngine;

public class Personaje : MonoBehaviour
{
    public float velocidad = 2f;
    public int vida = 1;

    protected Rigidbody2D rb;
    public SpriteRenderer sr;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public virtual void RecibirDano(int cantidad)
    {
        vida -= cantidad;

        if (vida <= 0)
        {
            Morir();
        }
    }

    protected virtual void Morir()
    {
        Destroy(gameObject);
    }
}