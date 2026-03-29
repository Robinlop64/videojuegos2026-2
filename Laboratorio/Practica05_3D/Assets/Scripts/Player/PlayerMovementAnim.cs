using UnityEngine;
public class PlayerMovementAnim : MonoBehaviour
{
    Animator anim;
    CharacterController controller;
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float speedValue = new Vector2(x, z).magnitude;
        anim.SetFloat("Speed", speedValue);
        Vector3 direction = new Vector3(x, 0, z);
        if (direction.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * 10f
            );
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("IsJumping", true);
        }
        else
        {

            anim.SetBool("IsJumping", false);
        }
    }
}