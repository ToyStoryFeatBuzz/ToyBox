using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace ToyBox.Build {
    public class BuildObject : MonoBehaviour {
        public List<Vector2> Offsets = new();

        [SerializeField] GameObject _placeHolder;
        [SerializeField] GameObject _finalObject;

        [FormerlySerializedAs("pickedEvent")] public UnityEvent OnPickedEvent = new();

        public bool DoErase = false;

        [FormerlySerializedAs("chosen")] public bool IsChosen = false;
        
        public GameObject _bombVisualPrefab; //Only necessary to be filled if it's a bomb

        public bool IsMovingPlatform;
        public GameObject MovingPart;
        public GameObject MovingPartPlaceholder;

        private void Start() {
            Place(false);
        }

        public bool ContainsPos(Vector2 pos) { // If Object contain the pos
            foreach (Vector2 offSet in Offsets) {
                if ((Vector2)transform.position + offSet == pos) {
                    return true;
                }
            }

            return false;
        }

        public void Place(bool place) { // Switch between place holder and the placed object
            _placeHolder.SetActive(!place);
            _finalObject.SetActive(place);
        }

        public void Pick() { // When chosen by player
            IsChosen = true;
            transform.parent = null;
            transform.localScale = Vector3.one;
            OnPickedEvent.Invoke();
        }

        public void RotateOffsets(float angle) { // Rotate the object by the angle
            List<Vector2> newOffsets = new();

            float angleRad = angle * Mathf.Deg2Rad;

            foreach (Vector2 offSet in Offsets) { // Rotation
                Vector2 rotated = new (
                    offSet.x * Mathf.Cos(angleRad) - offSet.y * Mathf.Sin(angleRad),
                    offSet.x * Mathf.Sin(angleRad) + offSet.y * Mathf.Cos(angleRad)
                );

                rotated.x = Mathf.Abs(rotated.x) < 1e-4f ? 0f : rotated.x;
                rotated.y = Mathf.Abs(rotated.y) < 1e-4f ? 0f : rotated.y;

                newOffsets.Add(rotated);
            }

            Offsets = newOffsets;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            float halfSize = (1f / 2f) - 0.02f;

            foreach (Vector2 offSet1 in Offsets)
            {
                var offSet = offSet1 + (Vector2)transform.position;
                Vector3 topLeft = new Vector3(offSet.x - halfSize, offSet.y + halfSize, 0f);
                Vector3 topRight = new Vector3(offSet.x + halfSize, offSet.y + halfSize, 0f);
                Vector3 bottomRight = new Vector3(offSet.x + halfSize, offSet.y - halfSize, 0f);
                Vector3 bottomLeft = new Vector3(offSet.x - halfSize, offSet.y - halfSize, 0f);

                Gizmos.DrawLine(topLeft, topRight);
                Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.DrawLine(bottomRight, bottomLeft);
                Gizmos.DrawLine(bottomLeft, topLeft);
                Gizmos.DrawLine(bottomLeft, topRight);
                Gizmos.DrawLine(topLeft, bottomRight);
            }

            
        }
    }
}
