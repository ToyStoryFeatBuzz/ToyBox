using System;
using UnityEngine;
using UnityEngine.UI;

namespace ToyBox.Menu {
    public class MenuSlider : MonoBehaviour {
        private Slider _slider;
        [SerializeField] private bool _isSelected;
        [SerializeField] private bool _isInteracting;

        [SerializeField] private int _offsetValue;

        [Header("Handler")] 
        [SerializeField] private GameObject _handler;
        [SerializeField] private Sprite _default;
        [SerializeField] private Sprite _selected;
        private Image _handlerRenderer;

        private void Start() {
            _slider = GetComponent<Slider>();
            
        }
    }
}