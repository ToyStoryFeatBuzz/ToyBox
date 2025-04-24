using ToyBox.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ToyBox.Menu {
    public class MenuSlider : MonoBehaviour,ISelectHandler, IDeselectHandler {
        private Slider _slider;
        [SerializeField] private bool _isSelected;
        [SerializeField] private bool _isInteracting;

        [SerializeField] private int _offsetValue;

        [Header("Handler")] 
        [SerializeField] private Image _handler;
        [SerializeField] private Sprite _default;
        [SerializeField] private Sprite _selected;
        
        MenuInputManager _inputManager => MenuInputManager.Instance;
        MenuInputDelegate _inputDelegate => MenuInputDelegate.Instance;

        private void Start() {
            _slider = GetComponent<Slider>();

            if (_inputManager != null) {
                _inputManager.OnSubmitEvent.Started += SelectSlider;
                _inputManager.OnCanceledEvent.Started += UnselectSlider;
            }
            else {
                _inputDelegate.PlayerInControl.OnSubmitEvent.Started += SelectSlider;
                _inputDelegate.PlayerInControl.OnCanceledEvent.Started += UnselectSlider;
            }

        }
        
        void SelectSlider() {
            if (_isSelected) {
                _isInteracting = true;
                _slider.interactable = false;
                _handler.sprite = _selected;
                if (_inputManager != null) {
                    _inputManager.OnNavigateEvent.Started += UpdateSlider;
                }
                else {
                    _inputDelegate.PlayerInControl.OnNavigateEvent.Started += UpdateSlider;
                }
            }
            else if(_isInteracting) {
                UnselectSlider();
            }
        }

        private void UnselectSlider() {
            if (!_isInteracting) {
                return;
            }

            _isInteracting = false;
            _slider.interactable = true;
            _slider.Select();
            if (_inputManager != null) {
                _inputManager.OnNavigateEvent.Started -= UpdateSlider;
            }
            else {
                _inputDelegate.PlayerInControl.OnNavigateEvent.Started -= UpdateSlider;
            }
        }

        private void UpdateSlider() {
            int value = Mathf.RoundToInt(_slider.value * 100);

            if (_inputManager != null) {
                if (_inputManager.NavigationValue.x > 0 || _inputManager.NavigationValue.y > 0) {
                    value += _offsetValue;
                }
                else if (_inputManager.NavigationValue.x < 0 || _inputManager.NavigationValue.y < 0) {
                    value -= _offsetValue;
                }
            }
            else {
                if (_inputDelegate.PlayerInControl.NavigationValue.x > 0 || _inputDelegate.PlayerInControl.NavigationValue.y > 0) {
                    value += _offsetValue;
                }
                else if (_inputDelegate.PlayerInControl.NavigationValue.x < 0 || _inputDelegate.PlayerInControl.NavigationValue.y < 0) {
                    value -= _offsetValue;
                }
            }

            _slider.value = (float)value / 100;
        }
        
        public void OnSelect(BaseEventData eventData) {
            _handler.sprite = _selected;
            _isSelected = true;
        }
        
        public void OnDeselect(BaseEventData eventData) {
            _isSelected = false;
            _handler.sprite = _default;
        }
    }
}