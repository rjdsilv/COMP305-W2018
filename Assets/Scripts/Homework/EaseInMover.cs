using UnityEngine;

public class EaseInMover : MonoBehaviour
{
    public Bounds bounds;

    private Rigidbody2D rigidBody;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (transform.position.x >= bounds.maxX)
        {
            rigidBody.velocity = new Vector2(0, 0);
        }
        else
        {
            rigidBody.AddForce(new Vector2(0.00030f, 0));
        }
    }
}
