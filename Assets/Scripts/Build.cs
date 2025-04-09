using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Build : MonoBehaviour
{
    public List<Vector2> offsets = new();

    public bool ContainsPos(Vector2 pos)
    {
        foreach (var v in offsets)
        {
            if ((Vector2)transform.position + v == pos) return true;
        }

        return false;
    }

    public void RotateOffsets(float angle)
    {
        List<Vector2> newOffsets = new();

        float angleRad = angle * Mathf.Deg2Rad;

        foreach (var v in offsets)
        {
            Vector2 rotated = new Vector2(
                v.x * Mathf.Cos(angleRad) - v.y * Mathf.Sin(angleRad),
                v.x * Mathf.Sin(angleRad) + v.y * Mathf.Cos(angleRad)
            );

            rotated.x = Mathf.Abs(rotated.x) < 1e-4f ? 0f : rotated.x;
            rotated.y = Mathf.Abs(rotated.y) < 1e-4f ? 0f : rotated.y;

            newOffsets.Add(rotated);
        }

        offsets = newOffsets;
    }
}
