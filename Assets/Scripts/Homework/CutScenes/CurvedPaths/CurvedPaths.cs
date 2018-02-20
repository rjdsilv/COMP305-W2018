using System.Collections.Generic;
using UnityEngine;

public class CurvedPaths : MonoBehaviour
{
    public Transform curvePoint;
    public Transform catmullRomButton;

    private bool drawCurve = false;
    private bool isLooping = true;
    private LineRenderer lineRenderer;
    private List<Transform> curvePoints = new List<Transform>();

    public enum CurveType
    {
        CATMULL_ROM
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        catmullRomButton.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (curvePoints.Count < 10)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10;
                curvePoints.Add(Instantiate(curvePoint, clickedPosition, Quaternion.identity));
            }

            if (curvePoints.Count >= 4)
            {
                catmullRomButton.gameObject.SetActive(true);
            }
        }
    }

    public void DrawCurve(CurveType curveType)
    {
        switch (curveType)
        {
            case CurveType.CATMULL_ROM:
                DrawCatmullRomCurve();
                break;
        }
    }

    private void DrawCatmullRomCurve()
    {
        lineRenderer.positionCount = 0;
        List<Vector3> drawingPositions = new List<Vector3>();
        for (int i = 0; i < curvePoints.Count; i++)
        {
            Vector3 p0 = curvePoints[ClampPos(i - 1)].position;
            Vector3 p1 = curvePoints[i].position;
            Vector3 p2 = curvePoints[ClampPos(i + 1)].position;
            Vector3 p3 = curvePoints[ClampPos(i + 2)].position;

            drawingPositions.Add(p1);

            float resolution = 0.01f;
            int loops = Mathf.FloorToInt(1f / resolution);

            lineRenderer.positionCount = loops;
            for (int j = 1; j <= loops; j++)
            {
                float t = j * resolution;
                drawingPositions.Add(GetCatmullRomPosition(t, p0, p1, p2, p3));
            }
        }
        lineRenderer.positionCount = drawingPositions.Count;
        lineRenderer.SetPositions(drawingPositions.ToArray());
        drawingPositions.Clear();
    }

    private int ClampPos(int pos)
    {
        if (pos < 0)
        {
            pos = curvePoints.Count - 1;
        }

        if (pos > curvePoints.Count)
        {
            pos = 1;
        }
        else if (pos > curvePoints.Count - 1)
        {
            pos = 0;
        }

        return pos;
    }

    private Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        //The cubic polynomial: a + b * t + c * t^2 + d * t^3
        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }
}
