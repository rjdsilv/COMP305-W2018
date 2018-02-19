using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 5;
    public float groundCheckRadius = 0.2f;
    public float jumpForce = 400;
    public Transform groundCheck;
    public LayerMask defineGroundCheck;

    private Rigidbody2D rBody;
    private SpriteRenderer sRender;
    private Animator animator;
    private bool isGrounded = false;

	// Use this for initialization
	void Start ()
    {
        rBody = GetComponent<Rigidbody2D>();
        sRender = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
	}

    void Update()
    {
        if (Input.GetAxis("Jump") > 0 && isGrounded)
        {
            animator.SetBool("Ground", false);
            rBody.AddForce(new Vector2(0, jumpForce));
        }
    }

    // Guaranteed to be called at defined intervals. USE FOR PHYSICS.
    void FixedUpdate ()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, defineGroundCheck);
        animator.SetBool("Ground", isGrounded);
        animator.SetFloat("vSpeed", rBody.velocity.y);

        float moveHoriz = Input.GetAxis("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(moveHoriz));
        rBody.velocity = new Vector2(moveHoriz * maxSpeed, rBody.velocity.y);

        if (moveHoriz > 0)
        {
            sRender.flipX = false;
        }
        else
        {
            sRender.flipX = true;
        }
	}
}
