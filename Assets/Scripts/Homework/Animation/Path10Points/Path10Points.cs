using System.Collections.Generic;
using UnityEngine;

public class Path10Points : MonoBehaviour
{
    public float speed = 2;
    public Transform pathPoint;
    public GameObject startButton;
    public GameObject pathWalker;

    private int fromPositionIndex = 0;
    private int toPositionIndex = 1;
    private bool _startWalk = false;
    private GameObject walker;
    private List<Vector3> _clickecPositions = new List<Vector3>();

    private void FixedUpdate()
    {
        if (_startWalk)
        {
            if ((walker.transform.position - _clickecPositions[toPositionIndex]).magnitude <= 0.025f)
            {
                if (toPositionIndex < _clickecPositions.Count - 1)
                {
                    fromPositionIndex++;
                    toPositionIndex++;
                    walker.GetComponent<Rigidbody2D>().velocity = (_clickecPositions[toPositionIndex] - _clickecPositions[fromPositionIndex]).normalized * speed;
                }
                else
                {
                    walker.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (_clickecPositions.Count < 10)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10;
                Instantiate(pathPoint, clickedPosition, Quaternion.identity);
                _clickecPositions.Add(clickedPosition);

                if (_clickecPositions.Count == 3)
                {
                    Instantiate(startButton);
                }
            }
        }
    }

    public void StartWalk()
    {
        _startWalk = true;
        walker = Instantiate(pathWalker, _clickecPositions[fromPositionIndex], Quaternion.identity);
        walker.GetComponent<Rigidbody2D>().velocity = (_clickecPositions[toPositionIndex] - _clickecPositions[fromPositionIndex]).normalized * speed;
    }
}
