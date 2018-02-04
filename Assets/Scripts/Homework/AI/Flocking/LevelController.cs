using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int numberOfFlocks;
    public int flockOffset;
    public int numberOfBoids;
    public float spawnRadius;
    public GameObject boid;

    public List<GameObject> AllBoids { get; set; }

    private void Start()
    {
        AllBoids = SpawnBoids();
    }

    private void Update()
    {
        foreach(var b in AllBoids)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 10;
            b.GetComponent<MemberMover>().MousePosition = pos;
        }
    }

    /// <summary>
    /// Spawn all the boids in the game world.
    /// </summary>
    /// <returns></returns>
    private List<GameObject> SpawnBoids()
    {
        List<GameObject> allBoids = new List<GameObject>(numberOfBoids);
        int multiFactor = 0;

        for (int f = 0; f < numberOfFlocks; f++)
        {
            float xOffset = Mathf.Pow(-1, f) * flockOffset * multiFactor;
            Debug.Log(xOffset);

            for (int i = 0; i < numberOfBoids; i++)
            {
                GameObject instantiatedBoid = Instantiate(
                    boid, 
                    new Vector3(Random.Range(-spawnRadius + xOffset, spawnRadius + xOffset), Random.Range(-spawnRadius, spawnRadius), 0),
                    Quaternion.identity
                );
                instantiatedBoid.GetComponent<MemberMover>().LevelController = GetComponent<LevelController>();
                allBoids.Add(instantiatedBoid);
            }

            if (f % 2 == 0)
            {
                multiFactor++;
            }
        }

        return allBoids;
    }
}
