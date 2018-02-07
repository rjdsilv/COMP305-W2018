using System.Collections;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public Bounds bounds;
    public float patrolSpeed;
    public float seekSpeed;
    public float attackSpeed;
    public float viewDistance;
    public float seekTime;

    private float startSeekTime;
    private State state;
    private Rigidbody2D enemyRigidbody;
    private SpriteRenderer enemyRenderer;

    private enum State
    {
        PATROL,
        SEEK,
        ATTACK
    }

	// Use this for initialization
	void Start ()
    {
        enemyRenderer = GetComponent<SpriteRenderer>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyRigidbody.velocity = Vector2.left;

        Patrol();
	}
	
	// FixedUpdate is called once per frame with time consistency.
	void FixedUpdate ()
    {
        // Change the enemy direction when reaching the bounds.
		if ((transform.position.x >= bounds.maxX) || (transform.position.x <= bounds.minX))
        {
            transform.Rotate(new Vector3(0, 0, 180));
            enemyRigidbody.velocity *= -1;
        }

        ApplyStateChanges();
    }

    bool IsSeeingPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(0).position, enemyRigidbody.velocity.normalized);
        return ((null != hit.collider) && (hit.collider.tag == "Player"));
    }

    void ApplyStateChanges()
    {
        switch(state)
        {
            case State.PATROL:
                // When sees the player, enter in attack mode!!!
                if (IsSeeingPlayer())
                {
                    Attack();
                }
                break;

            case State.ATTACK:
                // No longer seeing the player. If enemy was in attack mode, enter in seek mode.
                if (!IsSeeingPlayer())
                {
                    Seek();
                }
                break;

            case State.SEEK:
                // If the player is not being seen and enemy is seeking, keep in seeking state for the defined time. After that, enter in patrol mode.
                if (!IsSeeingPlayer())
                {
                    if (Time.time - startSeekTime > seekTime)
                    {
                        Patrol();
                    }
                }
                else
                {
                    Attack();
                }
                break;
        }
    }

    void Attack()
    {
        state = State.ATTACK;
        enemyRenderer.color = Color.red;
        enemyRigidbody.velocity = enemyRigidbody.velocity.normalized * attackSpeed;
    }

    void Seek()
    {
        state = State.SEEK;
        startSeekTime = Time.time;
        enemyRenderer.color = Color.yellow;
        enemyRigidbody.velocity = enemyRigidbody.velocity.normalized * seekSpeed;
    }

    void Patrol()
    {
        state = State.PATROL;
        enemyRenderer.color = Color.green;
        enemyRigidbody.velocity = enemyRigidbody.velocity.normalized * patrolSpeed;
    }
}
