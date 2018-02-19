using UnityEngine;

public class EaseOutMover : MonoBehaviour
{
    public Bounds bounds;

    private Rigidbody2D rigidBody;

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(9.85f, 0);
    }

    void FixedUpdate ()
    {
        if (transform.position.x >= bounds.maxX)
        {
            rigidBody.velocity = new Vector2(0, 0);
        }
        else
        {
            if (rigidBody.velocity.x > 0)
            {
                rigidBody.AddForce(new Vector2(-0.00030f, 0));
            }
        }
    }
}
