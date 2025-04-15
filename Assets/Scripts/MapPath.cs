using System.Collections.Generic;
using UnityEngine;

public class MapPath : MonoBehaviour
{
    public static MapPath Instance;



    public List<Vector2> points = new List<Vector2>()
    {
        new Vector2(0, 0),
        new Vector2(1, 0),
    };

    private void Awake()
    {
        Instance = this;
    }

    public float GetPlayerPositionOnPath(Transform obj)
    {
        if (points.Count < 2) return 0f;

        float advancement = 0f;

        float nearest = Mathf.Infinity;

        int index = 0;
        float progressOnEndLine = 0f;

        for (int i = 1; i < points.Count; i++)
        {
            Vector2 newPos = NearestPointOnFiniteLine(points[i - 1], points[i], (Vector2)obj.position);
            float dist = Vector2.SqrMagnitude(newPos - (Vector2)obj.position);
            if (dist < nearest)
            {
                nearest = dist;
                index = i;
                progressOnEndLine = Vector2.SqrMagnitude(points[i - 1] - newPos);
            }
        }

        if(index > 1)
        {
            for (int i = 1; i < index - 1; i++)
            {
                advancement += Vector2.SqrMagnitude(points[i - 1] - points[i]);
            }
        }
        

        return advancement + progressOnEndLine;
    }

    private Vector2 NearestPointOnFiniteLine(Vector2 start, Vector2 end, Vector2 pnt)
    {
        var line = (end - start);
        var len = line.magnitude;
        line.Normalize();

        var v = pnt - start;
        var d = Vector2.Dot(v, line);
        d = Mathf.Clamp(d, 0f, len);
        return start + line * d;
    }
}
