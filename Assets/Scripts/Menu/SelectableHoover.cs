using UnityEngine.EventSystems;

namespace ToyBox.Menu {
    public class SelectableHoover: SelectableNavigation, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler  {
        
        SelectableManager _selectableManager => SelectableManager.Instance;
        
        private bool _isSelected;
        
        public void OnPointerEnter(PointerEventData eventData) {
            if (!_selectable.enabled) return;
            _selectableManager.IsHoovering = true;
            _selectable.Select();
        }

        public void OnPointerExit(PointerEventData eventData) {
            _selectableManager.IsHoovering = false;
        }

        public void OnSelect(BaseEventData eventData) {
            _selectableManager.CurrentSelectable = _selectable;
            _isSelected = true;
        }

        public void OnDeselect(BaseEventData eventData) {
            _isSelected = false;
        }

        private void Update() {
            if (_isSelected || (_selectableManager.IsHoovering && _selectable == _selectableManager.CurrentSelectable)) {
                return;
            }
            _selectable.interactable = false;
            _selectable.interactable = true;
        }
    }
}