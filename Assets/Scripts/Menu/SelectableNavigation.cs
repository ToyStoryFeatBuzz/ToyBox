using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ToyBox.Menu {
    public class SelectableNavigation  : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {
        [SerializeField] StNavigation _navigation;

        private bool _isSelected;
        private Selectable _selectable;

        SelectableManager _selectableManager => SelectableManager.Instance;
        
        private void Start() {
            _selectable = GetComponent<Selectable>();
            _navigation.Init(_selectable);
        }

        private void Update() {
            _navigation.ApplyNavigation();
            Debug.Log(_selectable.navigation.selectOnUp);
        }


        public void OnPointerEnter(PointerEventData eventData) {
            if (!_selectable.enabled) return;
            _selectableManager.SelectableHooveredCount++;
            _selectable.Select();
        }

        public void OnPointerExit(PointerEventData eventData) {

        }

        public void OnSelect(BaseEventData eventData) {

        }

        public void OnDeselect(BaseEventData eventData) {

        }
    }

    
    [Serializable]
    public struct StNavigation {
        public Selectable Up;
        public Selectable Down;
        public Selectable Left;
        public Selectable Right;

        private Selectable _selectable;

        public void Init(Selectable selectable) {
            _selectable = selectable;
        }

        public void ApplyNavigation() {
            Navigation navigation = _selectable.navigation;
            navigation.selectOnUp = Up;
            navigation.selectOnDown = Down;
            navigation.selectOnLeft = Left;
            navigation.selectOnRight = Right;
            _selectable.navigation = navigation;
        }
    }
}