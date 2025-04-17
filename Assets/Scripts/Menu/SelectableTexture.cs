using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ToyBox.Menu {
    [RequireComponent(typeof(SelectableHoover))]
    public class SelectableTexture : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {
        private Image _renderer;
        
        [SerializeField] Sprite _default; 
        [SerializeField] Sprite _hoovered;
        
        bool _isHoovered;
        private bool _isSelected;

        private void Awake() {
            _renderer = GetComponent<Image>();
            _renderer.sprite = _default;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            _isHoovered = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            _isHoovered = false;
        }

        public void OnSelect(BaseEventData eventData) {
            _isSelected = true;
        }

        public void OnDeselect(BaseEventData eventData) {
            _isSelected = false;
        }

        private void Update() {
            if (_isSelected || _isHoovered) {
                _renderer.sprite = _hoovered;
            }
            else {
                _renderer.sprite = _default;
            }
        }
    }
}