using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        Debug.Log("Movimiento horizontal" +  h);
            
    }
}
