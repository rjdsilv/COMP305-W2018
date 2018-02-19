using UnityEngine;

public class Path10Clear : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Path10Points>().ClearAndReset();
    }
}
