using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public Bounds bounds;
    public float patrolSpeed;
    public float seekSpeed;
    public float attackSpeed;

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
        // Initial State.
        state = State.PATROL;

        // Setting the initial render status.
        enemyRenderer = GetComponent<SpriteRenderer>();
        enemyRenderer.color = Color.green;

        // Setting the initial velocity.
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyRigidbody.velocity = Vector2.left * patrolSpeed;
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
    }
}
