using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float regularSpeed = 5f;
    [SerializeField] float slowSpeed = 2f;
    float currentSpeed;


    Rigidbody2D rb;
    Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = regularSpeed;
    }

    void Update()
    {
        movement = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) movement.y += 1;
        if (Keyboard.current.sKey.isPressed) movement.y -= 1;
        if (Keyboard.current.aKey.isPressed) movement.x -= 1;
        if (Keyboard.current.dKey.isPressed) movement.x += 1;

        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentSpeed = slowSpeed;

        Debug.Log("Collision with: " + collision.gameObject.name);

        if (collision.gameObject.name == "Wall")
        {
            Debug.Log("Hit a wall!");
        }
        else if (collision.gameObject.name == "Rock")
        {
            Debug.Log("Hit a rock!");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        currentSpeed = regularSpeed;
    }

}
