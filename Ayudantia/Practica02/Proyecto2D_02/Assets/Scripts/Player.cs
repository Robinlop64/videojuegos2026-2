using UnityEngine;

public class Player : MonoBehaviour
{
    public bool enSuelo = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            enSuelo = true;

        Debug.Log("choque contra " + collision.gameObject.name);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            enSuelo = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            enSuelo = false;
    }
}
