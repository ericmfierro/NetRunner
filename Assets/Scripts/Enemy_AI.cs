using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy_AI : MonoBehaviour
{
    enum State { Patrol, Chase, Search, Frozen }
    State currentState = State.Patrol;
    State previousState;

    Transform player;
    Rigidbody2D rb;
    Animator animator;

    [Header("Movement")]
    [SerializeField] float patrolSpeed = 5f;
    [SerializeField] float chaseSpeed = 7f;
    [SerializeField] float patrolRadius = 4f;

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

    [Header("Tilemap")]
    [SerializeField] Tilemap floorTilemap;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (floorTilemap == null)
        {
            Tilemap[] maps = FindObjectsOfType<Tilemap>();

            foreach (Tilemap map in maps)
            {
                if (map.gameObject.name == "Tilemap")
                {
                    floorTilemap = map;
                    break;
                }
            }
        }

        ChooseNewPatrolPoint();
    }

    void Update()
    {
        if (player == null) return;

        if (currentState == State.Frozen)
        {
            moveDirection = Vector2.zero;
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

    void Patrol()
    {
        moveDirection = (patrolTarget - rb.position).normalized;

        if (Vector2.Distance(rb.position, patrolTarget) < 0.2f)
            ChooseNewPatrolPoint();
    }

    void ChooseNewPatrolPoint()
    {
        if (floorTilemap == null)
        {
            patrolTarget = rb.position;
            return;
        }

        for (int i = 0; i < 20; i++)
        {
            Vector2 randomPoint = rb.position + Random.insideUnitCircle * patrolRadius;
            Vector3Int cellPos = floorTilemap.WorldToCell(randomPoint);

            if (floorTilemap.HasTile(cellPos))
            {
                patrolTarget = floorTilemap.GetCellCenterWorld(cellPos);
                return;
            }
        }

        patrolTarget = rb.position;
    }

    void Chase()
    {
        moveDirection = ((Vector2)player.position - rb.position).normalized;
    }

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
        return currentState == State.Chase ? chaseSpeed : patrolSpeed;
    }

    void UpdateAnimator(Vector2 direction)
    {
        if (animator == null) return;

        bool isMoving = direction.sqrMagnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    bool CanSeePlayer()
    {
        if (player == null)
            return false;

        Vector2 toPlayer = (Vector2)player.position - rb.position;
        float distance = toPlayer.magnitude;

        if (distance > visionRange)
            return false;

        Vector2 forward = moveDirection;
        if (forward == Vector2.zero)
            forward = (patrolTarget - rb.position).normalized;

        float angle = Vector2.Angle(forward, toPlayer);
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

    public void SetFrozen(bool state)
    {
        if (state)
        {
            previousState = currentState;
            currentState = State.Frozen;
        }
        else
        {
            currentState = previousState;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        PlayerAbility ability = collision.GetComponent<PlayerAbility>();

        if (ability != null && ability.IsBusterActive())
        {
            Destroy(gameObject);
            return;
        }

        Player_Health health = collision.GetComponent<Player_Health>();
        if (health != null)
            health.TakeDamage(1);
    }
}
