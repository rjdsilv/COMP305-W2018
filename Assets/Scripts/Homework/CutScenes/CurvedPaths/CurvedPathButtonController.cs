using UnityEngine;

public class CurvedPathButtonController : MonoBehaviour
{
    public CurvedPaths.CurveType curveType;

    private void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<CurvedPaths>().DrawCurve(curveType);
    }
}
