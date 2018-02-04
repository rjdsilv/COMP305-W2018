using UnityEngine;

public class MemberMover : MonoBehaviour
{
    public MemberConfiguration memberConfiguration;

    public Vector3 MousePosition { get; set; }
    public LevelController LevelController { get; set; }

    private Rigidbody2D memberRigidbody;


	// Use this for initialization
	void Start ()
    {
        memberRigidbody = GetComponent<Rigidbody2D>();
	}

    // Called once per frame.
    void FixedUpdate()
    {
        memberRigidbody.velocity = transform.position - MousePosition;
        memberRigidbody.velocity = MousePosition * memberConfiguration.speed;

        var alignment = ComputeAlignment();
        var cohesion = ComputeCohesion();
        var separation = ComputeSeparation();

        memberRigidbody.velocity += 10 * alignment + 2 * cohesion + 7 * separation;
        memberRigidbody.velocity = memberRigidbody.velocity.normalized * memberConfiguration.speed;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Vector2 ComputeAlignment()
    {
        var alignmentVector = new Vector2();
        var neighbourCount = 0;

        foreach(var boid in LevelController.AllBoids)
        {
            if (boid != gameObject)
            {
                if (Vector3.Distance(transform.position, boid.transform.position) < memberConfiguration.alignmentRadius)
                {
                    alignmentVector += boid.GetComponent<Rigidbody2D>().velocity;
                    neighbourCount++;
                }
            }
        }

        if (neighbourCount == 0)
            return alignmentVector;

        alignmentVector /= neighbourCount;
        return alignmentVector.normalized;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Vector2 ComputeCohesion()
    {
        var cohesionVector = new Vector2();
        var neighbourCount = 0;

        foreach (var boid in LevelController.AllBoids)
        {
            if (boid != gameObject)
            {
                if (Vector3.Distance(transform.position, boid.transform.position) < memberConfiguration.cohesionRadius)
                {
                    cohesionVector.x += boid.transform.position.x;
                    cohesionVector.y += boid.transform.position.y;
                    neighbourCount++;
                }
            }
        }

        if (neighbourCount == 0)
            return cohesionVector;

        cohesionVector /= neighbourCount;
        return new Vector2(cohesionVector.x - transform.position.x, cohesionVector.y - transform.position.y).normalized;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Vector2 ComputeSeparation()
    {
        var separationVector = new Vector2();
        var neighbourCount = 0;

        foreach (var boid in LevelController.AllBoids)
        {
            if (boid != gameObject)
            {
                if (Vector3.Distance(transform.position, boid.transform.position) < memberConfiguration.separationRadius)
                {
                    separationVector.x += boid.transform.position.x - transform.position.x;
                    separationVector.y += boid.transform.position.y - transform.position.y;
                    neighbourCount++;
                }
            }
        }

        if (neighbourCount == 0)
            return separationVector;

        separationVector /= neighbourCount;
        separationVector *= -1;
        return separationVector.normalized;
    }
}
