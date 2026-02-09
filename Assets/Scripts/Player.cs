using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float regularSpeed = 5f;
    [SerializeField] float slowSpeed = 2f;

    float currentSpeed;

    Rigidbody2D rb;
    Animator animator;

    Vector2 rawInput;     // raw input for animator (-1, 0, 1)
    Vector2 movement;     // normalized movement for physics

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = regularSpeed;
    }

    void Update()
    {
        // --- RAW INPUT (DO NOT NORMALIZE) ---
        rawInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) rawInput.y = 1;
        if (Keyboard.current.sKey.isPressed) rawInput.y = -1;
        if (Keyboard.current.aKey.isPressed) rawInput.x = -1;
        if (Keyboard.current.dKey.isPressed) rawInput.x = 1;

        // --- ANIMATOR PARAMETERS ---
        bool isMoving = rawInput != Vector2.zero;

        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("moveX", rawInput.x);
        animator.SetFloat("moveY", rawInput.y);

        // --- MOVEMENT VECTOR (NORMALIZED AFTER ANIMATION LOGIC) ---
        movement = rawInput.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentSpeed = slowSpeed;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        currentSpeed = regularSpeed;
    }
}
