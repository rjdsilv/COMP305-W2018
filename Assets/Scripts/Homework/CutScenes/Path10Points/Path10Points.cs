using System.Collections.Generic;
using UnityEngine;

public class Path10Points : MonoBehaviour
{
    public float speed = 2;
    public Transform pathPoint;
    public GameObject startButton;
    public GameObject pathWalker;

    private int _fromPositionIndex = 0;
    private int _toPositionIndex = 1;
    private bool _startWalk = false;
    private GameObject walker;
    private List<Vector3> _clickecPositions = new List<Vector3>();
    private List<Transform> _pathPoints = new List<Transform>();

    private void FixedUpdate()
    {
        if (_startWalk)
        {
            if ((walker.transform.position - _clickecPositions[_toPositionIndex]).magnitude <= 0.05f)
            {
                if (_toPositionIndex < _clickecPositions.Count - 1)
                {
                    _fromPositionIndex++;
                    _toPositionIndex++;
                    walker.GetComponent<Rigidbody2D>().velocity = (_clickecPositions[_toPositionIndex] - _clickecPositions[_fromPositionIndex]).normalized * speed;
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
                _pathPoints.Add(Instantiate(pathPoint, clickedPosition, Quaternion.identity));
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
        walker = Instantiate(pathWalker, _clickecPositions[_fromPositionIndex], Quaternion.identity);
        walker.GetComponent<Rigidbody2D>().velocity = (_clickecPositions[_toPositionIndex] - _clickecPositions[_fromPositionIndex]).normalized * speed;
    }

    public void ClearAndReset()
    {
        Destroy(walker);
        foreach (Transform t in _pathPoints)
        {
            Destroy(t.gameObject);
        }
        _startWalk = false;
        _pathPoints.Clear();
        _clickecPositions.Clear();
        _fromPositionIndex = 0;
        _toPositionIndex = 1;
    }
}
