using System.Collections.Generic;
using ToyBox.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ToyBox.Menu {
    [RequireComponent(typeof(ScrollRect))]
    public class AutoScrollRect : MonoBehaviour {

        [SerializeField] private float _scrollSpeed = 10f;
        private ScrollRect _scrollRect;
        [SerializeField] List<Selectable> _scrollRectData = new();

        [SerializeField] Vector2 _newScrollPosition = Vector2.up;

        MenuInputManager _inputManager => MenuInputManager.Instance;

        private void Awake() {
            _scrollRect = GetComponent<ScrollRect>();
        }

        private void OnEnable() {
            if (_scrollRect != null) {
                _scrollRect.content.GetComponentsInChildren(_scrollRectData);
            }
        }

        private void Start() {
            _inputManager.OnNavigateEvent.Started += Scroll;

            if (_scrollRect != null) {
                _scrollRect.content.GetComponentsInChildren(_scrollRectData);
            }
            Debug.Log("Pieak");

            ScrollToSelected(true);
        }

        private void Update() {
            Scroll();

            if (!_inputManager.IsLastInputMouse) {
                Debug.Log(_newScrollPosition);
                _scrollRect.normalizedPosition = Vector2.Lerp(_scrollRect.normalizedPosition, _newScrollPosition, Time.unscaledDeltaTime * _scrollSpeed);
            }
            else {
                _newScrollPosition = _scrollRect.normalizedPosition;
                
            }
        }

        private void Scroll() {
            if (_scrollRectData.Count > 0) {
                ScrollToSelected(false);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void ScrollToSelected(bool isInstant) {
            int selectedID = -1;

            Selectable selectedItem = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null;

            if (selectedItem!= null) {
                selectedID = _scrollRectData.IndexOf(selectedItem);
            }

            if (selectedID <= -1) return;

            if (isInstant) {
                _scrollRect.normalizedPosition = new Vector2(0, 1 - selectedID / ((float)_scrollRectData.Count - 1));
                _newScrollPosition = _scrollRect.normalizedPosition;
            }
            else {
                _newScrollPosition = new Vector2(0, 1 - selectedID / ((float)_scrollRectData.Count - 1));
            }
        }
    }
}