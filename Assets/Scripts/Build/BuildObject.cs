using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace ToyBox.Build {
    public class BuildObject : MonoBehaviour {
        public List<Vector2> Offsets = new();

        [FormerlySerializedAs("placeHolder")] [SerializeField] GameObject _placeHolder;
        [FormerlySerializedAs("finalObject")] [SerializeField] GameObject _finalObject;

        [FormerlySerializedAs("pickedEvent")] public UnityEvent OnPickedEvent = new();

        [FormerlySerializedAs("erase")] public bool DoErase = false;

        [FormerlySerializedAs("chosen")] public bool IsChosen = false;

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
    }
}
