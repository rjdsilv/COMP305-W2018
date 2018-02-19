using UnityEngine;

public class Path10Start : MonoBehaviour {

    private void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Path10Points>().StartWalk();
    }
}
