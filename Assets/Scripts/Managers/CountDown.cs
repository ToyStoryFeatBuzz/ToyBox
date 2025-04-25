using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToyBox.Managers {
    public class CountDown : MonoBehaviour {
        [SerializeField] private Image _image;
        [SerializeField] private List<Sprite> _sprites;

        private void Start() {
            _image = GetComponent<Image>();
        }

        public void ToggleImage(bool toggle) {
            _image.enabled = toggle;
        }
        
        public void SetSprites(int index) {
            if (index > _sprites.Count - 1) {
                index = _sprites.Count-1;
            }

            if (index < 0) {
                index = 0;
            }

            _image.sprite = _sprites[index];
        }
    }
}