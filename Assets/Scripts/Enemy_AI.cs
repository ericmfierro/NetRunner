using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    enum State { Patrol, Chase, Search, Frozen }
    State currentState = State.Patrol;
    State previousState;

    Transform player;

    [Header("Movement")]
    [SerializeField] float patrolSpeed = 1.5f;
    [SerializeField] float chaseSpeed = 3f;
    Vector2 patrolTarget;
    Vector2 moveDirection = Vector2.right;

    [Header("Vision")]
    [SerializeField] float visionRange = 6f;
    [SerializeField] float visionAngle = 60f;
    [SerializeField] LayerMask visionBlockers;

    [Header("Search Memory")]
    [SerializeField] float searchTime = 2f;
    float searchTimer;
    Vector2 lastKnownPlayerPos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ChooseNewPatrolPoint();
    }

    void Update()
    {
        if (currentState == State.Frozen)
            return;

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
            case State.Patrol: Patrol(); break;
            case State.Chase: Chase(); break;
            case State.Search: Search(); break;
        }
    }

    // ---------------- PATROL ----------------
    void Patrol()
    {
        Vector2 dir = (patrolTarget - (Vector2)transform.position).normalized;
        moveDirection = dir;

        transform.position = Vector2.MoveTowards(transform.position, patrolTarget, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolTarget) < 0.2f)
            ChooseNewPatrolPoint();
    }

    void ChooseNewPatrolPoint()
    {
        patrolTarget = (Vector2)transform.position + Random.insideUnitCircle * 4f;
    }

    // ---------------- CHASE ----------------
    void Chase()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        moveDirection = dir;

        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    // ---------------- SEARCH ----------------
    void Search()
    {
        Vector2 dir = (lastKnownPlayerPos - (Vector2)transform.position).normalized;
        moveDirection = dir;

        transform.position = Vector2.MoveTowards(transform.position, lastKnownPlayerPos, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, lastKnownPlayerPos) < 0.2f)
        {
            searchTimer -= Time.deltaTime;
            if (searchTimer <= 0)
                currentState = State.Patrol;
        }
    }

    // ---------------- VISION SYSTEM ----------------
    bool CanSeePlayer()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;

        if (distance > visionRange)
            return false;

        float angle = Vector2.Angle(moveDirection, directionToPlayer);
        if (angle > visionAngle / 2f)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, distance, visionBlockers);

        if (hit.collider != null)
            return false;

        return true;
    }

    // ---------------- FREEZE (ENCRYPTION BLAST) ----------------
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

    // ---------------- COLLISION SYSTEM ----------------
    void OnCollisionEnter2D(Collision2D collision)
    {
        // If player has blade active → enemy dies
        PlayerAbility ability = collision.gameObject.GetComponent<PlayerAbility>();
        if (ability != null && ability.bladeActive)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise enemy damages player
        if (collision.collider.CompareTag("Player"))
        {
            Player_Health health = collision.collider.GetComponent<Player_Health>();
            if (health != null)
                health.TakeDamage(1);
        }
    }

    // ---------------- DEBUG VISION GIZMOS ----------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)moveDirection * visionRange);
        }
    }
}
