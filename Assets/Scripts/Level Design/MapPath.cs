using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.LevelDesign {
    public class MapPath : MonoBehaviour {
        public static MapPath Instance;

        public float totalLength { get; private set; }

        public List<Vector2> points = new List<Vector2>() {
            new Vector2(0, 0),
            new Vector2(1, 0),
        };

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            totalLength = GetTotalLength();
        }

        public float
            GetPlayerPositionOnPath(Transform obj) // Get the progression from the start according to the Map Path
        {
            if (points.Count < 2) return 0f;

            float advancement = 0f;

            float nearest = Mathf.Infinity;

            int index = 0;
            float progressOnEndLine = 0f;

            for (int i = 1; i < points.Count; i++) {
                Vector2 newPos = NearestPointOnFiniteLine(points[i - 1], points[i], (Vector2)obj.position);
                float dist = Vector2.SqrMagnitude(newPos - (Vector2)obj.position);
                if (dist < nearest) {
                    nearest = dist;
                    index = i;
                    progressOnEndLine = Vector2.SqrMagnitude(points[i - 1] - newPos);
                }
            }

            if (index > 1) {
                for (int i = 1; i < index - 1; i++) {
                    advancement += Vector2.SqrMagnitude(points[i - 1] - points[i]);
                }
            }

            return advancement + progressOnEndLine;
        }

        public float GetPlayerAdvancement(Transform obj) // Progression between 0 and 1 on the Map
        {
            float dist = GetPlayerPositionOnPath(obj);

            return dist / totalLength;
        }

        private float GetTotalLength() {
            float total = 0f;
            for (int i = 1; i < points.Count; i++) {
                total += Vector2.SqrMagnitude(points[i - 1] - points[i]);
            }

            return total;
        }

        private Vector2
            NearestPointOnFiniteLine(Vector2 start, Vector2 end, Vector2 pnt) // Get a line and snap a position to it
        {
            Vector2 line = (end - start);
            float len = line.magnitude;
            line.Normalize();

            Vector2 v = pnt - start;
            float d = Vector2.Dot(v, line);
            d = Mathf.Clamp(d, 0f, len);
            return start + line * d;
        }
    }
}