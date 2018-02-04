using System.Collections.Generic;
using UnityEngine;

public class Member : MonoBehaviour
{
    public float speed;
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public Level level;
    public MemberConfig config;

    Vector3 wanderTarget;

	// Use this for initialization
	void Start ()
    {
        level = FindObjectOfType<Level>();
        config = FindObjectOfType<MemberConfig>();
        position = transform.position;
        velocity = new Vector3(Random.Range(-speed, speed), Random.Range(-speed, speed), 0);
    }

    void Update()
    {
        acceleration = Combine();
        acceleration = Vector3.ClampMagnitude(acceleration, config.maxAcceleration);
        velocity = velocity + acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, config.maxVelocity);
        position = position + velocity * Time.deltaTime;
        transform.SetPositionAndRotation(position, Quaternion.identity);
        WrapAround(ref position, level.bounds.minX, level.bounds.maxX);
    }

    protected Vector3 Wander()
    {
        float jitter = config.wanderJitter * Time.deltaTime;
        wanderTarget += new Vector3(RandomBinomial() * jitter, RandomBinomial() * jitter, 0);
        wanderTarget = wanderTarget.normalized;
        wanderTarget *= config.wanderRadius;
        Vector3 targetInLocalSpace = wanderTarget + new Vector3(0, config.wanderDistance, 0);
        Vector3 targetInWorldSpace = transform.TransformPoint(targetInLocalSpace);
        targetInWorldSpace -= position;
        return targetInWorldSpace.normalized;
    }

    protected Vector3 Cohesion()
    {
        Vector3 cohesionVector = new Vector3();
        int countOfOurMembers = 0;
        List<Member> neighbours = level.GetNeighbours(this, config.cohesionRadius);

        if (neighbours.Count == 0)
        {
            return cohesionVector;
        }

        foreach (var member in neighbours)
        {
            if (IsInFOV(member.position))
            {
                cohesionVector += member.position;
                countOfOurMembers++;
            }
        }

        if (countOfOurMembers == 0)
        {
            return cohesionVector;
        }

        cohesionVector /= countOfOurMembers;
        cohesionVector -= position;
        cohesionVector = Vector3.Normalize(cohesionVector);

        return cohesionVector;
    }

    protected Vector3 Alignment()
    {
        Vector3 alignVector = new Vector3();
        List<Member> members = level.GetNeighbours(this, config.alignmentRadius);
        if (members.Count == 0)
        {
            return alignVector;
        }

        foreach (var member in members)
        {
            if (IsInFOV(member.position))
            {
                alignVector += member.velocity;
            }
        }

        return alignVector.normalized;
    }

    protected Vector3 Separation()
    {
        Vector3 separationVector = new Vector3();
        List<Member> members = level.GetNeighbours(this, config.separationRadius);

        if (members.Count == 0)
        {
            return separationVector;
        }

        foreach (var member in members)
        {
            if (IsInFOV(member.position))
            {
                Vector3 movingTowards = position - member.position;

                if (movingTowards.magnitude > 0)
                {
                    separationVector += movingTowards.normalized / movingTowards.magnitude;
                }
            }
        }

        return separationVector.normalized;
    }

    virtual protected Vector3 Combine()
    {
        Vector3 finalVec = config.cohesionPriority * Cohesion() + config.wanderPriority * Wander() + config.alignmentPriority * Alignment() + config.separationPriority * Separation();
        return finalVec;
    }

    void WrapAround(ref Vector3 vector, float min, float max)
    {
        vector.x = WrapAroundFloat(vector.x, min, max);
        vector.y = WrapAroundFloat(vector.y, min, max);
        vector.z = WrapAroundFloat(vector.z, min, max);
    }

    float WrapAroundFloat(float value, float min, float max)
    {
        if (value > max)
        {
            value = min;
        }
        else if (value < min)
        {
            value = max;
        }

        return value;
    }

    float RandomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }

    bool IsInFOV(Vector3 vec)
    {
        return Vector3.Angle(this.velocity, vec - position) <= config.maxFOV;
    }
}
