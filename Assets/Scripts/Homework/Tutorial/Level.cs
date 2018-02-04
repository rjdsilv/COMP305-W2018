using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int numberOfMembers;
    public float spawnRadius;
    public Transform member;
    public List<Member> members;
    public Bounds bounds;

	// Use this for initialization
	void Start ()
    {
        members = new List<Member>();
        Spawn();
        members.AddRange(FindObjectsOfType<Member>());
	}

    void Spawn()
    {
        for (int i = 0; i < numberOfMembers; i++)
        {
            Instantiate(member, new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0), Quaternion.identity);
        }
    }

    public List<Member> GetNeighbours(Member member, float radius)
    {
        List<Member> neighboursFound = new List<Member>();

        foreach (var otherMember in members)
        {
            if (otherMember == member)
            {
                continue;
            }

            if (Vector3.Distance(member.position, otherMember.position) <= radius)
            {
                neighboursFound.Add(otherMember);
            }
        }

        return neighboursFound;
    }
}
