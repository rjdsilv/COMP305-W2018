using UnityEngine;

public class LinearMover : MonoBehaviour
{
    public Bounds bounds;

    private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(5, 0);
	}

    void FixedUpdate()
    {
        if(transform.position.x >= bounds.maxX)
        {
            rigidBody.velocity = new Vector2(0, 0);
        }
    }
}
