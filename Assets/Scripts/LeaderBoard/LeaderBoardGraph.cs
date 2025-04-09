using System;
using UnityEngine;

public class LeaderBoardGraph : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private int _rounds = 5;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void SetMatch()
    {
        lineRenderer.positionCount = _rounds;
    }
    
    private void DrawLines()
    {
        for (int i = 0; i < _rounds; i++)
        {
            float x = i * 2;
            float y = Mathf.Sin(i);
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
