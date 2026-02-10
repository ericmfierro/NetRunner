using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    enum State { Patrol, Chase, Search, Frozen }
    State currentState = State.Patrol;
    State previousState;

    Transform player;
    Rigidbody2D rb;
    Animator animator;

    [Header("Movement")]
    [SerializeField] float patrolSpeed = 1.5f;
    [SerializeField] float chaseSpeed = 3f;

    Vector2 patrolTarget;
    Vector2 moveDirection;

    [Header("Vision")]
    [SerializeField] float visionRange = 6f;
    [SerializeField] float visionAngle = 60f;
    [SerializeField] LayerMask visionBlockers;

    [Header("Search")]
    [SerializeField] float searchTime = 2f;
    float searchTimer;
    Vector2 lastKnownPlayerPos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        ChooseNewPatrolPoint();
    }

    void Update()
    {
        if (currentState == State.Frozen)
        {
            UpdateAnimator(Vector2.zero);
            return;
        }

        if (CanSeePlayer())
        {
            currentState = State.Chase;
            lastKnownPlayerPos = player.position;
        }
        else if (currentState == State.Chase)
        {
            currentState = State.Search;
            searchTimer = searchTime;
        }

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Search:
                Search();
                break;
        }

        UpdateAnimator(moveDirection);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * GetCurrentSpeed() * Time.fixedDeltaTime);
    }

    // ---------------- PATROL ----------------
    void Patrol()
    {
        moveDirection = (patrolTarget - rb.position).normalized;

        if (Vector2.Distance(rb.position, patrolTarget) < 0.2f)
            ChooseNewPatrolPoint();
    }

    void ChooseNewPatrolPoint()
    {
        patrolTarget = rb.position + Random.insideUnitCircle * 4f;
    }

    // ---------------- CHASE ----------------
    void Chase()
    {
        moveDirection = ((Vector2)player.position - rb.position).normalized;
    }

    // ---------------- SEARCH ----------------
    void Search()
    {
        moveDirection = (lastKnownPlayerPos - rb.position).normalized;

        if (Vector2.Distance(rb.position, lastKnownPlayerPos) < 0.2f)
        {
            searchTimer -= Time.deltaTime;
            if (searchTimer <= 0)
                currentState = State.Patrol;
        }
    }

    float GetCurrentSpeed()
    {
        if (currentState == State.Chase)
            return chaseSpeed;

        return patrolSpeed;
    }

    // ---------------- ANIMATION ----------------
    void UpdateAnimator(Vector2 direction)
    {
        bool isMoving = direction != Vector2.zero;

        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    // ---------------- VISION ----------------
    bool CanSeePlayer()
    {
        Vector2 toPlayer = (Vector2)player.position - rb.position;
        float distance = toPlayer.magnitude;

        if (distance > visionRange)
            return false;

        float angle = Vector2.Angle(moveDirection, toPlayer);
        if (angle > visionAngle / 2f)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(
            rb.position,
            toPlayer.normalized,
            distance,
            visionBlockers
        );

        return hit.collider == null;
    }

    // ---------------- FREEZE ----------------
    public void Freeze(float duration)
    {
        previousState = currentState;
        StartCoroutine(FreezeRoutine(duration));
    }

    IEnumerator FreezeRoutine(float time)
    {
        currentState = State.Frozen;
        yield return new WaitForSeconds(time);
        currentState = previousState;
    }

    // ---------------- COLLISION ----------------
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerAbility ability = collision.gameObject.GetComponent<PlayerAbility>();
        if (ability != null && ability.bladeActive)
        {
            Destroy(gameObject);
            return;
        }

        if (collision.collider.CompareTag("Player"))
        {
            Player_Health health = collision.collider.GetComponent<Player_Health>();
            if (health != null)
                health.TakeDamage(1);
        }
    }
}
