using UnityEngine;

public class Player : Personaje
{
    public bool enSuelo = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enSuelo = true;
        }

        Debug.Log("Choque contra " + collision.gameObject.name);
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
}