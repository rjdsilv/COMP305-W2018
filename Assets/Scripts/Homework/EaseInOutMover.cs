using UnityEngine;

public class EaseInOutMover : MonoBehaviour
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
            if (rigidBody.position.x <= 0)
            {
                rigidBody.AddForce(new Vector2(0.00060f, 0));
            }
            else
            {
                rigidBody.AddForce(new Vector2(-0.00060f, 0));
            }
        }
    }
}
