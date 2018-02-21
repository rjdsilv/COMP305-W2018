using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedPaths : MonoBehaviour
{
    public float curveResolution = 0.01f;
    public float timeFrame = 0.04f;
    public Vector3 tangent = new Vector3(0, 0, 0);
    public Transform curvePoint;
    public Transform curveWalker;
    public Transform catmullRomButton;
    public Transform bezierButton;
    public Transform cubicHermiteButton;

    private LineRenderer lineRenderer;
    private List<Transform> curvePoints = new List<Transform>();

    public enum CurveType
    {
        CATMULL_ROM, BEZIER, CUBIC_HERMITE
    }

    private void Start()
    {
        curveWalker = Instantiate(curveWalker);
        lineRenderer = GetComponent<LineRenderer>();
        catmullRomButton.gameObject.SetActive(false);
        bezierButton.gameObject.SetActive(false);
        cubicHermiteButton.gameObject.SetActive(false);
        curveWalker.gameObject.SetActive(false);
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

            if (curvePoints.Count >= 3)
            {
                catmullRomButton.gameObject.SetActive(true);
                bezierButton.gameObject.SetActive(true);
                cubicHermiteButton.gameObject.SetActive(true);
            }
        }
    }

    public void DrawCurve(CurveType curveType)
    {
        curveWalker.gameObject.SetActive(true);
        switch (curveType)
        {
            case CurveType.CATMULL_ROM:
                StartCoroutine(DrawCatmullRomCurve());
                break;

            case CurveType.BEZIER:
                StartCoroutine(DrawBezierCurve());
                break;

            case CurveType.CUBIC_HERMITE:
                StartCoroutine(DrawCubicHermiteCurve());
                break;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// CATMULL ROM

    private IEnumerator DrawCatmullRomCurve()
    {
        for (int i = 0; i < curvePoints.Count - 1; i++)
        {
            Vector3 p0 = curvePoints[ClampPos(i - 1)].position;
            Vector3 p1 = curvePoints[i].position;
            Vector3 p2 = curvePoints[ClampPos(i + 1)].position;
            Vector3 p3 = curvePoints[ClampPos(i + 2)].position;

            for (int j = 1, loops = Mathf.FloorToInt(1f / curveResolution); j <= loops; j++)
            {
                float t = j * curveResolution;
                curveWalker.position = GetCatmullRomPosition(t, p0, p1, p2, p3);
                yield return new WaitForSeconds(timeFrame);
            }
        }
    }

    private int ClampPos(int pos)
    {
        if (pos < 0)
        {
            pos = 0;
        }
        else  if (pos > curvePoints.Count - 1)
        {
            pos = curvePoints.Count - 1;
        }

        return pos;
    }

    private Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;
        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// BEZIER

    private IEnumerator DrawBezierCurve()
    {
        curveWalker.position = curvePoints[0].position;
        for (int i = 1, loops = Mathf.FloorToInt(1f / curveResolution); i <= loops; i++)
        {
            curveWalker.position = GetBezierPosition(curvePoints, i * curveResolution);
            yield return new WaitForSeconds(timeFrame);

        }
        curveWalker.position = curvePoints[curvePoints.Count - 1].position;
    }

    private Vector3 GetBezierPosition(List<Transform> positions, float t)
    {
        Vector3 p = Vector3.zero;
        for (int i = 0; i < positions.Count; i++)
        {
            p += positions[i].position * CalculateBernsteinPolynomial(i, positions.Count - 1, t);
        }
        return p;
    }

    private float CalculateBernsteinPolynomial(int i, int n, float t)
    {
        return CalculateNewtonBinomial(n, i) * Mathf.Pow(t, i) * Mathf.Pow(1f - t, n - i);
    }

    private float CalculateNewtonBinomial(int n, int i)
    {
        float binomial = 1.0f;

        for (int k = n; k > Mathf.Max(i, n - i); k--)
        {
            binomial *= k;
        }
        binomial *= CalculateFactorial(Mathf.Min(n, i));

        return binomial;
    }

    private int CalculateFactorial(int n)
    {
        if (n == 0)
        {
            return 1;
        }

        return CalculateFactorial(n - 1);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// CUBIC HERMITE

    private IEnumerator DrawCubicHermiteCurve()
    {
        List<Vector3> drawingPositions = new List<Vector3>();

        for (int i = 0; i < curvePoints.Count - 1; i++)
        {
            for (int j = 1, loops = Mathf.FloorToInt(1f / curveResolution); j <= loops; j++)
            {
                curveWalker.position = GetCubicHermitePosition(j * curveResolution, curvePoints[i].position, curvePoints[ClampPos(i + 1)].position);
                yield return new WaitForSeconds(timeFrame);
            }
        }
    }

    private Vector3 GetCubicHermitePosition(float t, Vector3 p0, Vector3 p1)
    {
        float h0 = 2 * Mathf.Pow(t, 3) - 3 * Mathf.Pow(t, 2) + 1;
        float h1 = Mathf.Pow(t, 3) - 2 * Mathf.Pow(t, 2) + t;
        float h2 = -2 * Mathf.Pow(t, 3) + 3 * Mathf.Pow(t, 2);
        float h3 = Mathf.Pow(t, 3) - Mathf.Pow(t, 2);
        Vector3 pt = (h0 * p0 + h1 * tangent + h2 * p1 + h3 * tangent);

        return pt;
    }
}
